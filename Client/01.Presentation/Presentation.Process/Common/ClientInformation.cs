using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.PopupServiceRef;
using Utilities.Common;
using System.Data;

namespace Presentation.Process.Common
{
    /// <summary>
    /// @Truongnx on 20120905
    /// Lớp này lưu các thông tin thuộc về một phiên làm việc của người dùng, với mục đích truy xuất nhanh:
    /// - Thông tin người dùng: id, username, fullname, chi nhánh,...
    /// - Thông tin tham số hệ thống: đường dẫn thư mục,...
    /// - Thông tin tham số nghiệp vụ: ...
    /// </summary>
    public static class ClientInformation
    {
        // Các thông tin tham số hệ thống
        public static DataTable DataTableConfig;
        public static string Company;
        public static string ClientType;
        public static string WorkingDir;
        public static string ConfigDir;
        public static string DataDir;
        public static string HelpDir;
        public static string ImagesDir;
        public static string LanguagesDir;
        public static string TempDir;
        public static string IconName;
        public static string ShortName;
        public static string FullName;
        public static string Theme;
        public static string LanguageList;
        public static string VersionDir;
        public static string BackupVersionDir;
        public static string CurrentVersionDir;
        public static string DefaultVersionDir;
        public static string OtaVersionDir;
        public static string Log4NetConfig;
        public static string Log4NetUpdConfig;
        public static string Log4NetOutput;
        public static string ServerList;
        public static string ServerName;
        public static string ServerIP;
        public static string ServerPort;
        public static string License;
        public static string Version;
        public static string SessionId;        
        public static string IpAddress;
        public static string MacAddress;
        public static string DonViToChuc;

        public static string RequestSecurityKey = "mfclient.ngvgroup.vn";
        public static string ResponsetSecurityKey = "mfserver.ngvgroup.vn";

        // Các thông tin thuộc về người dùng
        public static int IdNguoiSuDung;
        public static string TenDangNhap;
        public static string HoTen;
        public static string LoaiNguoiSuDung;
        // Đơn vị giao dịch người dùng lựa chọn khi đăng nhập (phòng)
        public static int IdDonViGiaoDich;
        public static string MaDonViGiaoDich;
        public static string TenDonViGiaoDich;
        public static string TinhTPDonViGiaoDich;
        // Đơn vị người dùng lựa chọn khi giao dịch (chi nhánh)
        public static int IdDonVi;
        public static string MaDonVi;
        public static string TenDonVi;
        // Đơn vị người dùng thuộc về (thông tin quản lý)
        public static int IdDonViQuanLy;
        public static string MaDonViQuanLy;
        public static string TenDonViQuanLy;
        // Tổ chức người dùng thuộc về (cấp cao nhất của đơn vị)
        public static int IdToChu;
        public static string MaToChuc;
        public static string TenToChuc;
        // Các thông tin khác
        public static string PhuongPhapHachToan;
        public static int SoLuongBanGhi = 10;
        public static string NgonNgu;

        // Các chức năng, quyền của người dùng
        public static ChucNangDto[] ListChucNang;

        // Các phòng giao dịch được can thiệp dữ liệu
        public static List<DM_DON_VI> ListPhongGD;

        // Các thông tin về phiên làm việc của người dùng
        public static DateTime ClientSessionStartTime;
        public static DateTime ClientSessionCurrentTime;       

        // Các thông tin thuộc về tham số nghiệp vụ
        public static string NgayLamViecTruoc;
        public static string NgayLamViecHienTai;
        public static string NgayLamViecSau;
        public static string MaDongNoiTe;
        public static string MaQuocGiaBanDia;

        // Chức năng vừa chọn
        public static string ChucNangVuaChon;
        public static string FormCase;

        // Thông tin popup đang được sử dụng
        public static SimplePopupResponse SimplePopup;

        // Trạng thái xử lý hiện tại
        // Trạng thái này được set để các chức năng khác kiểm tra bổ sung
        public static string OperationStatus;

        public static void Release()
        {
            /*
            ClientInformation.WorkingDir = null;
            ClientInformation.ConfigDir = null;
            ClientInformation.DataDir = null;
            ClientInformation.ImagesDir = null;
            ClientInformation.LanguagesDir = null;
            ClientInformation.TempDir = null;
            ClientInformation.Log4NetConfig = null;
            ClientInformation.Log4NetOutput = null;
            ClientInformation.ServerIP = null;
            ClientInformation.ServerPort = null;
            ClientInformation.License = null;
            ClientInformation.Version = null;
            ClientInformation.SessionId = null;
            ClientInformation.IpAddress = null;
            ClientInformation.MacAddress = null;
            
            ClientInformation.RequestSecurityKey = null;
            ClientInformation.ResponsetSecurityKey = null;
            */

            // Các thông tin thuộc về người dùng
            ClientInformation.IdNguoiSuDung = 0;
            ClientInformation.TenDangNhap = null;
            ClientInformation.HoTen = null;
            ClientInformation.LoaiNguoiSuDung = null;
            ClientInformation.IdDonViGiaoDich = 0;
            ClientInformation.MaDonViGiaoDich = null;
            ClientInformation.TenDonViGiaoDich = null;
            ClientInformation.TinhTPDonViGiaoDich = null;
            // Đơn vị người dùng thuộc về
            ClientInformation.IdDonVi = 0;
            ClientInformation.MaDonVi = null;
            ClientInformation.TenDonVi = null;
            // Tổ chức người dùng thuộc về (cấp cao nhất của đơn vị)
            ClientInformation.IdToChu = 0;
            ClientInformation.MaToChuc = null;
            ClientInformation.TenToChuc = null;
            // Các thông tin khác
            ClientInformation.PhuongPhapHachToan = null;
            ClientInformation.SoLuongBanGhi = 10;
            ClientInformation.NgonNgu = null;

            // Các chức năng, quyền của người dùng
            ClientInformation.ListChucNang = null;

            // Các thông tin về phiên làm việc của người dùng
            ClientInformation.ClientSessionStartTime = DateTime.Now;
            ClientInformation.ClientSessionCurrentTime = DateTime.Now;       

            // Các thông tin thuộc về tham số nghiệp vụ
            ClientInformation.NgayLamViecTruoc = null;
            ClientInformation.NgayLamViecHienTai = null;
            ClientInformation.NgayLamViecSau = null;
            ClientInformation.MaDongNoiTe = null;
            ClientInformation.MaQuocGiaBanDia = null;

            // Chức năng vừa chọn
            ClientInformation.ChucNangVuaChon = null;
            ClientInformation.FormCase = null;

            // Thông tin popup đang được sử dụng
            ClientInformation.SimplePopup = null;

            // Trạng thái xử lý hiện tại
            // Trạng thái này được set để các chức năng khác kiểm tra bổ sung
            ClientInformation.OperationStatus = null;
        }
    }

    public class Server
    {
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string ServerCode { get; set; }

        public List<Server> getServerList(string ServerListPattern)
        {
            List<Server> lstServer = new List<Server>();

            List<string> ServerObject = ServerListPattern.SplitByDelimiter("#").ToList();
            foreach (string item in ServerObject)
            {
                List<string> ServerItem = item.SplitByDelimiter("@").ToList();
                Server server = new Server();
                if (ServerItem.Count == 3)
                {
                    server.ServerName = ServerItem[0];
                    server.ServerIP = ServerItem[1];
                    server.ServerPort = ServerItem[2];
                    server.ServerCode = "";
                    lstServer.Add(server);
                }
                else if (ServerItem.Count == 4)
                {
                    server.ServerName = ServerItem[0];
                    server.ServerIP = ServerItem[1];
                    server.ServerPort = ServerItem[2];
                    server.ServerCode = ServerItem[3];
                    lstServer.Add(server);
                }
            }
            return lstServer;
        }
    }

    public class Language
    {
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageStatus { get; set; } // 1: Su dung; hay 0: khong su dung

        public List<Language> getLanguageList(string LanguageListPattern)
        {
            List<Language> lstLanguage = new List<Language>();

            List<string> LanguageObject = LanguageListPattern.SplitByDelimiter("#").ToList();
            foreach (string item in LanguageObject)
            {
                List<string> LanguageItem = item.SplitByDelimiter("@").ToList();
                Language language = new Language();
                if (LanguageItem.Count == 3)
                {
                    language.LanguageName = LanguageItem[0];
                    language.LanguageCode = LanguageItem[1];
                    language.LanguageStatus = LanguageItem[2];
                    lstLanguage.Add(language);
                }
                else if (LanguageItem.Count == 4)
                {
                    language.LanguageName = LanguageItem[0];
                    language.LanguageCode = LanguageItem[1];
                    language.LanguageStatus = LanguageItem[2];
                    lstLanguage.Add(language);
                }
            }
            return lstLanguage;
        }
    }
}
