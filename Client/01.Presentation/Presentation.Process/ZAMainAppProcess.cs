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
using System.Xml;
using System.IO;

namespace Presentation.Process
{
    public class ZAMainAppProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static ZAMainAppServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// Yêu cầu 
        /// </summary>
        static ZAMainAppProcess()
        {
            //new ClientInitProcess().docThongTinCauHinhClient(1);
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());
            Client = new ZAMainAppServiceClient(basicHttpBinding, endpointAddress);

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

        public void createSession()
        {
            // Đọc thông tin cấu hình từ Client
            new ClientInitProcess().docThongTinCauHinhClient(1);
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            LLogging.WriteLog("Process createSession after validation: ", LLogging.LogType.SYS, DateTime.Now.ToLongTimeString());
            // Khởi tạo và gán các giá trị cho request
            SessionRequest request = Common.Utilities.PrepareRequest(new SessionRequest());
            request.License = ClientInformation.License;
            request.Version = ClientInformation.Version;

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_LOGIN;
            request.Action = DatabaseConstant.Action.DANG_NHAP;

            // Lấy kết quả trả về
            SessionResponse response = Client.getSession(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            string sessionId = response.SessionId;
            ClientInformation.SessionId = sessionId;
        }

        public bool doLogin(string userName, string passWord, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            LoginRequest request = Common.Utilities.PrepareRequest(new LoginRequest());
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
            Common.Utilities.ValidResponse(request, response);

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

        public bool doLoginWithSession(string userName, string passWord, ref NGON_NGU_DTO ngonNguDTO, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            LoginRequest request = Common.Utilities.PrepareRequest(new LoginRequest());
            request.License = ClientInformation.License;
            request.Version = ClientInformation.Version;
            request.UserName = userName.ToUpper();
            request.PassWord = passWord;

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_LOGIN;
            request.Action = DatabaseConstant.Action.DANG_NHAP;
            request.Company = ApplicationConstant.layDonViSuDung(ClientInformation.Company);

            if (ClientInformation.ClientType.Equals(ApplicationConstant.ClientType.DESKTOP.layGiaTri()))
            {
                request.PhienBanResource = LLanguage.SearchResourceByKey("U.PhienBan");
                request.PhienBanMessage = LLanguage.SearchResourceByKey("M.PhienBan");
            }
            if (ClientInformation.ClientType.Equals(ApplicationConstant.ClientType.WEB.layGiaTri()))
            {
                UserInformation userInfo = HttpContext.Current.Session["UserInformation"] as UserInformation;
                request.PhienBanResource = userInfo.U_PhienBan;
                request.PhienBanMessage = userInfo.M_PhienBan;
            }

            // Ghi log
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Login at Client : " + userName);
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, "Login at Client : " + userName);

            // Lấy kết quả trả về            
            LoginResponse response = Client.doLoginWithSession(request);            

            // Kiểm tra kết quả trả về
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, "Before ValidResponse");
            Common.Utilities.ValidResponse(request, response);
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, "After ValidResponse");

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
                if (ClientInformation.ClientType.Equals(ApplicationConstant.ClientType.DESKTOP.layGiaTri()))
                {
                    ClientInformation.SessionId = sessionId;

                    ClientInformation.TenDangNhap = response.NguoiSuDung.MA_DANG_NHAP;
                    ClientInformation.ListChucNang = response.ListChucNang;
                    if (response.NguoiSuDung != null)
                    {
                        ClientInformation.IdNguoiSuDung = response.NguoiSuDung.ID;
                        ClientInformation.HoTen = response.NguoiSuDung.TEN_DAY_DU;
                        ClientInformation.MaDonViQuanLy = response.NguoiSuDung.MA_DVI_QLY;
                        ClientInformation.LoaiNguoiSuDung = response.NguoiSuDung.PHAN_LOAI_NSD;
                    }
                    if (response.DonViGiaoDich != null)
                    {
                        ClientInformation.IdDonViGiaoDich = response.DonViGiaoDich.ID;
                        ClientInformation.MaDonViGiaoDich = response.DonViGiaoDich.MA_DVI;
                        ClientInformation.TenDonViGiaoDich = response.DonViGiaoDich.TEN_GDICH;
                        ClientInformation.PhuongPhapHachToan = response.DonViGiaoDich.MA_HACH_TOAN;
                    }
                    if (response.DonVi != null)
                    {
                        ClientInformation.IdDonVi = response.DonVi.ID;
                        ClientInformation.MaDonVi = response.DonVi.MA_DVI;
                        ClientInformation.TenDonVi = response.DonVi.TEN_GDICH;
                    }
                    if (response.DonViQuanLy != null)
                    {
                        ClientInformation.IdDonViQuanLy = response.DonViQuanLy.ID;
                        ClientInformation.MaDonViQuanLy = response.DonViQuanLy.MA_DVI;
                        ClientInformation.TenDonViQuanLy = response.DonViQuanLy.TEN_GDICH;
                    }
                    if (response.DonViRoot != null)
                    {
                        ClientInformation.IdToChu = response.DonViRoot.ID;
                        ClientInformation.MaToChuc = response.DonViRoot.MA_DVI;
                        ClientInformation.TenToChuc = response.DonViRoot.TEN_GDICH;
                    }
                    if (response.ListPhongGD != null)
                    {
                        ClientInformation.ListPhongGD = response.ListPhongGD.ToList();
                    }
                    if (response.NgonNguDTO != null)
                    {
                        ngonNguDTO = response.NgonNguDTO;
                    }

                    if (response.ShowConfigDTO != null)
                    {
                        WriteShowConfig(response.ShowConfigDTO);
                    }

                    ClientInformation.NgayLamViecTruoc = response.NgayLamViecTruoc;
                    ClientInformation.NgayLamViecHienTai = response.NgayLamViecHienTai;
                    ClientInformation.NgayLamViecSau = response.NgayLamViecSau;
                    ClientInformation.MaDongNoiTe = response.MaDongNoiTe;
                    ClientInformation.MaQuocGiaBanDia = response.MaQuocGiaBanDia;
                }
                else
                {
                    UserInformation userInfo = new UserInformation();
                    userInfo.SessionId = sessionId;
                    userInfo.TenDangNhap = response.NguoiSuDung.MA_DANG_NHAP;
                    userInfo.ListChucNang = response.ListChucNang;
                    if (response.NguoiSuDung != null)
                    {
                        userInfo.IdNguoiSuDung = response.NguoiSuDung.ID;
                        userInfo.HoTen = response.NguoiSuDung.TEN_DAY_DU;
                        userInfo.MaDonViQuanLy = response.NguoiSuDung.MA_DVI_QLY;
                        userInfo.LoaiNguoiSuDung = response.NguoiSuDung.PHAN_LOAI_NSD;
                    }
                    if (response.DonViGiaoDich != null)
                    {
                        userInfo.IdDonViGiaoDich = response.DonViGiaoDich.ID;
                        userInfo.MaDonViGiaoDich = response.DonViGiaoDich.MA_DVI;
                        userInfo.TenDonViGiaoDich = response.DonViGiaoDich.TEN_GDICH;
                        userInfo.PhuongPhapHachToan = response.DonViGiaoDich.MA_HACH_TOAN;
                    }
                    if (response.DonVi != null)
                    {
                        userInfo.IdDonVi = response.DonVi.ID;
                        userInfo.MaDonVi = response.DonVi.MA_DVI;
                        userInfo.TenDonVi = response.DonVi.TEN_GDICH;
                    }
                    if (response.DonViQuanLy != null)
                    {
                        userInfo.IdDonViQuanLy = response.DonViQuanLy.ID;
                        userInfo.MaDonViQuanLy = response.DonViQuanLy.MA_DVI;
                        userInfo.TenDonViQuanLy = response.DonViQuanLy.TEN_GDICH;
                    }
                    if (response.DonViRoot != null)
                    {
                        userInfo.IdToChu = response.DonViRoot.ID;
                        userInfo.MaToChuc = response.DonViRoot.MA_DVI;
                        userInfo.TenToChuc = response.DonViRoot.TEN_GDICH;
                    }
                    if (response.ListPhongGD != null)
                    {
                        userInfo.ListPhongGD = response.ListPhongGD.ToList();
                    }
                    if (response.NgonNguDTO != null)
                    {
                        ngonNguDTO = response.NgonNguDTO;
                    }
                    userInfo.NgayLamViecTruoc = response.NgayLamViecTruoc;
                    userInfo.NgayLamViecHienTai = response.NgayLamViecHienTai;
                    userInfo.NgayLamViecSau = response.NgayLamViecSau;
                    userInfo.MaDongNoiTe = response.MaDongNoiTe;
                    userInfo.MaQuocGiaBanDia = response.MaQuocGiaBanDia;
                    HttpContext.Current.Session["UserInformation"] = userInfo;
                }
                ClientInformation.Company = ApplicationConstant.layGiaTri(response.Company);
                return true;
            }
        }

        /// <summary>
        /// Đăng xuất khỏi hệ thống
        /// </summary>
        public void doLogout()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            LogoutRequest request = Common.Utilities.PrepareRequest(new LogoutRequest());

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_LOGOUT;
            request.Action = DatabaseConstant.Action.DANG_XUAT;

            // Ghi log
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Logout at Client : " + request.UserName);

            // Lấy kết quả trả về
            LogoutResponse response = Client.doLogout(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
        }

        public void doCheckSession()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            SessionRequest request = Common.Utilities.PrepareRequest(new SessionRequest());
            
            // Ghi log
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Logout at Client : " + request.UserName);

            // Lấy kết quả trả về
            SessionResponse response = Client.doCheckSession(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
        }

        public void WriteShowConfig(DSACH_SHOW_CONFIG objShowConfig)
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\ShowConfig.xml"))
                    File.Delete(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\ShowConfig.xml");
                XmlTextWriter writer = new XmlTextWriter(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\ShowConfig.xml", System.Text.Encoding.UTF8);
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "SYS WriteShowConfig");
                writer.WriteStartDocument(true);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("root");
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "SYS StartCreateNode");
                CreateNode("form", objShowConfig.DSACH_SHOW.ToList(), writer);
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "SYS EndCreateNode");
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, "ERR WriteShowConfig");
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        public void CreateNode(string typeNode, List<SHOW_CONFIG_DTO> lstShow, XmlTextWriter writer)
        {
            List<SHOW_CONFIG_DTO> lstShowChild = null;
            List<SHOW_CONFIG_DTO> lstForm = null;
            switch (typeNode)
            {
                case "form":
                    lstForm = lstShow.GroupBy(f => f.FORM).Select(f => f.FirstOrDefault()).ToList();
                    foreach (SHOW_CONFIG_DTO objShow in lstForm)
                    {
                        writer.WriteStartElement("form");
                        writer.WriteAttributeString("Name", objShow.FORM);
                        lstShowChild = lstShow.Where(f => f.FORM.Equals(objShow.FORM)).ToList();
                        CreateNode("case", lstShowChild, writer);
                        writer.WriteEndElement();
                    }
                    break;
                case "case":
                    lstForm = lstShow.GroupBy(f => f.CASE).Select(f => f.FirstOrDefault()).ToList();
                    foreach (SHOW_CONFIG_DTO objShow in lstForm)
                    {
                        writer.WriteStartElement("case");
                        writer.WriteAttributeString("Name", objShow.CASE);
                        lstShowChild = lstShow.Where(f => f.CASE.Equals(objShow.CASE)).ToList();
                        CreateNode("control", lstShowChild, writer);
                        writer.WriteEndElement();
                    }
                    break;
                case "control":
                    lstForm = lstShow.GroupBy(f => f.CONTROL).Select(f => f.FirstOrDefault()).ToList();
                    foreach (SHOW_CONFIG_DTO objShow in lstForm)
                    {
                        writer.WriteStartElement("control");
                        writer.WriteAttributeString("Name", objShow.CONTROL);
                        lstShowChild = lstShow.Where(f => f.CONTROL.Equals(objShow.CONTROL)).ToList();
                        CreateNode("property", lstShowChild, writer);
                        writer.WriteEndElement();
                    }
                    break;
                case "property":
                    foreach (SHOW_CONFIG_DTO objShow in lstShow)
                    {
                        writer.WriteStartElement("property");
                        writer.WriteAttributeString("Name", objShow.PROPERTY);
                        writer.WriteString(objShow.VALUE);
                        writer.WriteEndElement();
                    }
                    break;
            }
        }
    }
}
