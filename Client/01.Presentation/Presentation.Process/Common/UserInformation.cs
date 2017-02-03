using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using System.Data;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.PopupServiceRef;
//using PresentationAspNet.MVC.PopupServiceRef;

namespace Presentation.Process.Common
{
    /// <summary>
    /// @Truongnx on 20120905
    /// Lớp này lưu các thông tin thuộc về một phiên làm việc của người dùng, với mục đích truy xuất nhanh:
    /// - Thông tin người dùng: id, username, fullname, chi nhánh,...
    /// - Thông tin tham số hệ thống: đường dẫn thư mục,...
    /// - Thông tin tham số nghiệp vụ: ...
    /// </summary>
    public class UserInformation
    {
        //Ngon ngu
        public static string MessageResources_en;
        public static string MessageResources_vi;
        public static string UIResources_en;
        public static string UIResources_vi;

        // Các thông tin tham số hệ thống
        public DataTable DataTableConfig;
        public string Company;
        public string WorkingDir;
        public string ConfigDir;
        public string DataDir;
        public string HelpDir;
        public string ImagesDir;
        public string LanguagesDir;
        public string TempDir;
        public string IconName;
        public string ShortName;
        public string FullName;
        public string VersionDir;
        public string BackupVersionDir;
        public string CurrentVersionDir;
        public string DefaultVersionDir;
        public string OtaVersionDir;
        public string Log4NetConfig;
        public string Log4NetUpdConfig;
        public string Log4NetOutput;
        public string ServerList;
        public string ServerName;
        public string ServerIP;
        public string ServerPort;
        public string License;
        public string Version;
        public string SessionId;
        public string IpAddress;
        public string MacAddress;
        public string DonViToChuc;

        public string RequestSecurityKey = "mfclient.ngvgroup.vn";
        public string ResponsetSecurityKey = "mfserver.ngvgroup.vn";

        public string SecurityType;

        // Các thông tin thuộc về người dùng
        public int IdNguoiSuDung;
        public string TenDangNhap;
        public string HoTen;
        public string LoaiNguoiSuDung;
        // Đơn vị giao dịch người dùng lựa chọn khi đăng nhập (phòng)
        public int IdDonViGiaoDich;
        public string MaDonViGiaoDich;
        public string TenDonViGiaoDich;
        // Đơn vị người dùng lựa chọn khi giao dịch (chi nhánh)
        public int IdDonVi;
        public string MaDonVi;
        public string TenDonVi;
        // Đơn vị người dùng thuộc về (thông tin quản lý)
        public int IdDonViQuanLy;
        public string MaDonViQuanLy;
        public string TenDonViQuanLy;
        // Tổ chức người dùng thuộc về (cấp cao nhất của đơn vị)
        public int IdToChu;
        public string MaToChuc;
        public string TenToChuc;
        // Các thông tin khác
        public string PhuongPhapHachToan;
        public int SoLuongBanGhi = 10;
        public string NgonNgu;

        // Các chức năng, quyền của người dùng
        public ChucNangDto[] ListChucNang;

        // Các phòng giao dịch được can thiệp dữ liệu
        public List<DM_DON_VI> ListPhongGD;

        // Version ngôn ngữ cho bản web
        public string U_PhienBan;
        public string M_PhienBan;

        // Các thông tin về phiên làm việc của người dùng
        public DateTime ClientSessionStartTime;
        public DateTime ClientSessionCurrentTime;

        // Các thông tin thuộc về tham số nghiệp vụ
        public string NgayLamViecTruoc;
        public string NgayLamViecHienTai;
        public string NgayLamViecSau;
        public string MaDongNoiTe;
        public string MaQuocGiaBanDia;

        // Chức năng vừa chọn
        public string ChucNangVuaChon;
        public string FormCase;

        // Thông tin popup đang được sử dụng
        //public SimplePopupResponse SimplePopup;
        // Thông tin popup đang được sử dụng
        public SimplePopupResponse SimplePopup;
        // Trạng thái xử lý hiện tại
        // Trạng thái này được set để các chức năng khác kiểm tra bổ sung
        public string OperationStatus;

        public UserInformation()
        { 
            
            //hoangadd
            NgonNgu = "vi";
            //endhoangadd

            // Các thông tin tham số hệ thống
            DataTableConfig = ClientInformation.DataTableConfig;
            Company = ClientInformation.Company;
            WorkingDir = ClientInformation.WorkingDir;
            ConfigDir = ClientInformation.ConfigDir;
            DataDir = ClientInformation.DataDir;
            HelpDir = ClientInformation.HelpDir;
            ImagesDir = ClientInformation.ImagesDir;
            LanguagesDir = ClientInformation.LanguagesDir;
            TempDir = ClientInformation.TempDir;
            IconName = ClientInformation.IconName;
            ShortName = ClientInformation.ShortName;
            FullName = ClientInformation.FullName;
            VersionDir = ClientInformation.VersionDir;
            BackupVersionDir = ClientInformation.BackupVersionDir;
            CurrentVersionDir = ClientInformation.CurrentVersionDir;
            DefaultVersionDir = ClientInformation.DefaultVersionDir;
            OtaVersionDir = ClientInformation.OtaVersionDir;
            Log4NetConfig = ClientInformation.Log4NetConfig;
            Log4NetUpdConfig = ClientInformation.Log4NetUpdConfig;
            Log4NetOutput = ClientInformation.Log4NetOutput;
            ServerList = ClientInformation.ServerList;
            ServerName = ClientInformation.ServerName;
            ServerIP = ClientInformation.ServerIP;
            ServerPort = ClientInformation.ServerPort;
            License = ClientInformation.License;
            Version = ClientInformation.Version;
            //SessionId = ClientInformation.SessionId;
            //IpAddress = ClientInformation.IpAddress;
            //MacAddress = ClientInformation.MacAddress;
            //DonViToChuc = ClientInformation.DonViToChuc;

            RequestSecurityKey = ClientInformation.RequestSecurityKey;
            ResponsetSecurityKey = ClientInformation.ResponsetSecurityKey;

            //SecurityType = ClientInformation.SecurityType;

            // Các thông tin thuộc về người dùng
            //IdNguoiSuDung =ClientInformation.IdNguoiSuDung;
            //TenDangNhap =ClientInformation.TenDangNhap;
            //HoTen =ClientInformation.HoTen;
            //LoaiNguoiSuDung =ClientInformation.LoaiNguoiSuDung;
            // Đơn vị giao dịch người dùng lựa chọn khi đăng nhập (phòng) 
            //IdDonViGiaoDich =ClientInformation.IdDonViGiaoDich;
            //MaDonViGiaoDich =ClientInformation.MaDonViGiaoDich;
            //TenDonViGiaoDich =ClientInformation.TenDonViGiaoDich;
            // Đơn vị người dùng lựa chọn khi giao dịch (chi nhánh) 
            //IdDonVi =ClientInformation.IdDonVi;
            //MaDonVi =ClientInformation.MaDonVi;
            //TenDonVi =ClientInformation.TenDonVi;
            // Đơn vị người dùng thuộc về (thông tin quản lý) 
            //IdDonViQuanLy =ClientInformation.IdDonViQuanLy;
            //MaDonViQuanLy =ClientInformation.MaDonViQuanLy;
            //TenDonViQuanLy =ClientInformation.TenDonViQuanLy;
            
            // Tổ chức người dùng thuộc về (cấp cao nhất của đơn vị) 
            //IdToChu = ClientInformation.IdToChu;
            //MaToChuc = ClientInformation.MaToChuc;
            //TenToChuc = ClientInformation.TenToChuc;
            
            // Các thông tin khác
            //PhuongPhapHachToan = ClientInformation.PhuongPhapHachToan;
            SoLuongBanGhi = ClientInformation.SoLuongBanGhi;
            //NgonNgu = ClientInformation.NgonNgu;

            // Các chức năng, quyền của người dùng 
            //ListChucNang = ClientInformation.ListChucNang;

            // Các phòng giao dịch được can thiệp dữ liệu 
            //ListPhongGD = ClientInformation.ListPhongGD;

            // Các thông tin về phiên làm việc của người dùng
            //ClientSessionStartTime = ClientInformation.ClientSessionStartTime;
            //ClientSessionCurrentTime = ClientInformation.ClientSessionCurrentTime;

            // Các thông tin thuộc về tham số nghiệp vụ 
            //NgayLamViecTruoc = ClientInformation.NgayLamViecTruoc;
            //NgayLamViecHienTai = ClientInformation.NgayLamViecHienTai;
            //NgayLamViecSau = ClientInformation.NgayLamViecSau;
            //MaDongNoiTe = ClientInformation.MaDongNoiTe;
            //MaQuocGiaBanDia = ClientInformation.MaQuocGiaBanDia;

            // Chức năng vừa chọn 
            //ChucNangVuaChon = ClientInformation.ChucNangVuaChon;
            //FormCase = ClientInformation.FormCase;

            // Thông tin popup đang được sử dụng 
            //SimplePopup = ClientInformation.SimplePopup;

            // Trạng thái xử lý hiện tại 
            // Trạng thái này được set để các chức năng khác kiểm tra bổ sung
            //OperationStatus = ClientInformation.OperationStatus;
        }

        public void Release()
        {

            // Các thông tin thuộc về người dùng
            IdNguoiSuDung = 0;
            TenDangNhap = null;
            HoTen = null;
            LoaiNguoiSuDung = null;
            IdDonViGiaoDich = 0;
            MaDonViGiaoDich = null;
            TenDonViGiaoDich = null;
            // Đơn vị người dùng thuộc về
            IdDonVi = 0;
            MaDonVi = null;
            TenDonVi = null;
            // Tổ chức người dùng thuộc về (cấp cao nhất của đơn vị)
            IdToChu = 0;
            MaToChuc = null;
            TenToChuc = null;
            // Các thông tin khác
            PhuongPhapHachToan = null;
            SoLuongBanGhi = 10;
            NgonNgu = null;

            // Các chức năng, quyền của người dùng
            ListChucNang = null;

            // Các thông tin về phiên làm việc của người dùng
            ClientSessionStartTime = DateTime.Now;
            ClientSessionCurrentTime = DateTime.Now;

            // Các thông tin thuộc về tham số nghiệp vụ
            NgayLamViecTruoc = null;
            NgayLamViecHienTai = null;
            NgayLamViecSau = null;
            MaDongNoiTe = null;
            MaQuocGiaBanDia = null;

            // Chức năng vừa chọn
            ChucNangVuaChon = null;
            FormCase = null;

            // Thông tin popup đang được sử dụng
            //SimplePopup = null;

            // Trạng thái xử lý hiện tại
            // Trạng thái này được set để các chức năng khác kiểm tra bổ sung
            OperationStatus = null;
        }
    }

}
