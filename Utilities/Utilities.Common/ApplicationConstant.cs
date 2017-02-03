using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Common
{
    /// <summary>
    /// @Truongnx on 20120905
    /// Lưu các thông tin global mang tính hệ thống
    /// - Các định dạng dữ liệu hệ thống
    /// - Lịch làm việc của hệ thống
    /// - Trạng thái trả về của Server cho Client
    /// - Key language trả về của Server cho Client
    /// </summary>
    public static class ApplicationConstant
    {

        /// <summary>
        /// Định dạng ngày tháng mặc định dùng để xử lý và trao đổi dữ liệu trong ứng dụng
        /// </summary>
        public const string defaultDateTimeFormat = "yyyyMMdd";

        /// <summary>
        /// Đường dẫn mặc định của thư viện ảnh
        /// </summary>
        public const string defaultImageSource = "pack://application:,,,/Utilities.Common;component/Images/";

        /// <summary>
        /// Loại lịch: Lịch nghỉ theo âm lịch, dương lịch hoặc lịch làm việc vào ngày nghỉ
        /// </summary>
        public enum LoaiLich { LICH_AM, LICH_DUONG, LICH_LVIEC };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiLich">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoaiLich loaiLich)
        {
            switch (loaiLich)
            {
                case LoaiLich.LICH_AM: return "LICH_AM";
                case LoaiLich.LICH_DUONG: return "LICH_DUONG";
                case LoaiLich.LICH_LVIEC: return "LICH_LVIEC";
                default: return "";
            }
        }

        /// <summary>
        /// Kết trả về từ Server cho Client
        /// </summary>
        public enum ResponseStatus { KHONG_THANH_CONG, THANH_CONG };
        /// <summary>
        /// Lấy giá trị kiểu int của enum
        /// </summary>
        public static int layGiaTri(this ResponseStatus ketQua)
        {
            switch (ketQua)
            {
                case ResponseStatus.THANH_CONG: return 1;
                case ResponseStatus.KHONG_THANH_CONG: return 0;
                default: return 1;
            }
        }

        public enum ClientType { DESKTOP, WEB, WEBCLIENT };
        public static string layGiaTri(this ClientType clientType)
        {
            switch (clientType)
            {
                case ClientType.DESKTOP: return "DESKTOP";
                case ClientType.WEB: return "WEB";
                case ClientType.WEBCLIENT: return "WEBCLIENT";
                default: return "";
            }
        }

        public enum FormatType { EMAIL, PHONE, CMND, HOKHAU, DKKD };
        public static string layGiaTri(this FormatType item)
        {
            switch (item)
            {
                case FormatType.EMAIL: return "EMAIL";
                case FormatType.PHONE: return "PHONE";
                case FormatType.CMND: return "CMND";
                case FormatType.HOKHAU: return "HOKHAU";
                case FormatType.DKKD: return "DKKD";
                default: return "";
            }
        }

        /// <summary>
        /// Message trả về từ Server cho Client nói chung
        /// </summary>
        public enum CommonResponseMessage
        {
            M_ResponseMessage_Common_ThanhCong,
            M_ResponseMessage_Common_KhongThanhCong,
            M_ResponseMessage_Common_NoResponse,
            M_ResponseMessage_Common_ResponseIdIsNotMatch,
            M_ResponseMessage_Common_InvalidSecurityResponseKey,
            M_ResponseMessage_Common_InvalidSecurityRequestKey,
            M_ResponseMessage_Common_InvalidOrExpiredOperationSession,
            M_ResponseMessage_Common_InvalidOrExpiredUserSession,
            M_ResponseMessage_Common_InvalidCompany,
            M_ResponseMessage_Common_InvalidLicense,
            M_ResponseMessage_Common_InvalidVersion,
            M_ResponseMessage_Common_NotLatestVersion,
            M_ResponseMessage_Common_LoseSession,
            M_ResponseMessage_Common_LockDataInvalid,
            M_ResponseMessage_Common_UnlockDataInvalid,
            M_ResponseMessage_Common_InvalidWorkingDay,

            M_ResponseMessage_Common_KhongTonTaiDuLieu,
            M_ResponseMessage_Common_LoiXuLyDuLieu,
            M_ResponseMessage_Common_RangBuocDuLieu,
            M_ResponseMessage_Common_DaPhatSinhNghiepVu,
            M_ResponseMessage_Common_TrangThaiNghiepVuKhongPhuHop,
            M_ResponseMessage_Common_DangChoDuyet,
            M_ResponseMessage_Common_ChuaDuyet,
            M_ResponseMessage_Common_DaDuyet,
            M_ResponseMessage_Common_DaThoaiDuyet,
            M_ResponseMessage_Common_DaTuChoiDuyet,

            M_ResponseMessage_Common_PhamViDuLieuKhongPhuHop,

            M_ResponseMessage_Common_NotExistCMND,
            M_ResponseMessage_Common_ExistCMND,
            M_ResponseMessage_Common_NotExistCMNDGiaDinh,
            M_ResponseMessage_Common_ExistCMNDGiaDinh,
            M_ResponseMessage_Common_NotExistCMNDNguoiThuaKe,
            M_ResponseMessage_Common_ExistCMNDNguoiThuaKe,
            M_ResponseMessage_Common_NotExistCMNDNguoiBaoLanh,
            M_ResponseMessage_Common_ExistCMNDNguoiBaoLanh,
            M_ResponseMessage_Common_NotExistDKKD,
            M_ResponseMessage_Common_ExistDKKD,
            M_ResponseMessage_Common_NotExistSoHoKhau,
            M_ResponseMessage_Common_ExistSoHoKhau,
            M_ResponseMessage_Common_NotExistSoHoKhauGiaDinh,
            M_ResponseMessage_Common_ExistSoHoKhauGiaDinh,
            M_ResponseMessage_Common_NotExistSoHoKhauNguoiThuaKe,
            M_ResponseMessage_Common_ExistSoHoKhauNguoiThuaKe,
            M_ResponseMessage_Common_NotExistSoHoKhauNguoiBaoLanh,
            M_ResponseMessage_Common_ExistSoHoKhauNguoiBaoLanh
        };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// Giá trị trả về là key_language cho các thông báo
        /// </summary>
        /// <param name="loaiLich">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CommonResponseMessage commonResponseMessage)
        {
            switch (commonResponseMessage)
            {
                case CommonResponseMessage.M_ResponseMessage_Common_ThanhCong: return "M.ResponseMessage.Common.ThanhCong";
                case CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong: return "M.ResponseMessage.Common.KhongThanhCong";
                case CommonResponseMessage.M_ResponseMessage_Common_NoResponse: return "M.ResponseMessage.Common.NoResponse";
                case CommonResponseMessage.M_ResponseMessage_Common_ResponseIdIsNotMatch: return "M.ResponseMessage.Common.ResponseIdIsNotMatch";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidSecurityResponseKey: return "M.ResponseMessage.Common.InvalidSecurityResponseKey";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidSecurityRequestKey: return "M.ResponseMessage.Common.InvalidSecurityRequestKey";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidOrExpiredOperationSession: return "M.ResponseMessage.Common.InvalidOrExpiredOperationSession";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidOrExpiredUserSession: return "M.ResponseMessage.Common.InvalidOrExpiredUserSession";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidCompany: return "M.ResponseMessage.Common.InvalidCompany";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidLicense: return "M.ResponseMessage.Common.InvalidLicense";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidVersion: return "M.ResponseMessage.Common.InvalidVersion";
                case CommonResponseMessage.M_ResponseMessage_Common_NotLatestVersion: return "M.ResponseMessage.Common.NotLatestVersion";
                case CommonResponseMessage.M_ResponseMessage_Common_LoseSession: return "M.ResponseMessage.Common.LoseSession";
                case CommonResponseMessage.M_ResponseMessage_Common_LockDataInvalid: return "M.ResponseMessage.Common.LockDataInvalid";
                case CommonResponseMessage.M_ResponseMessage_Common_UnlockDataInvalid: return "M.ResponseMessage.Common.UnlockDataInvalid";
                case CommonResponseMessage.M_ResponseMessage_Common_InvalidWorkingDay: return "M.ResponseMessage.Common.InvalidWorkingDay";

                case CommonResponseMessage.M_ResponseMessage_Common_KhongTonTaiDuLieu: return "M.ResponseMessage.Common.KhongTonTaiDuLieu";
                case CommonResponseMessage.M_ResponseMessage_Common_LoiXuLyDuLieu: return "M.ResponseMessage.Common.LoiXuLyDuLieu";
                case CommonResponseMessage.M_ResponseMessage_Common_RangBuocDuLieu: return "M.ResponseMessage.Common.RangBuocDuLieu";
                case CommonResponseMessage.M_ResponseMessage_Common_DaPhatSinhNghiepVu: return "M.ResponseMessage.Common.DaPhatSinhNghiepVu";
                case CommonResponseMessage.M_ResponseMessage_Common_TrangThaiNghiepVuKhongPhuHop: return "M.ResponseMessage.Common.TrangThaiNghiepVuKhongPhuHop";
                case CommonResponseMessage.M_ResponseMessage_Common_DangChoDuyet: return "M.ResponseMessage.Common.DangChoDuyet";
                case CommonResponseMessage.M_ResponseMessage_Common_ChuaDuyet: return "M.ResponseMessage.Common.ChuaDuyet";
                case CommonResponseMessage.M_ResponseMessage_Common_DaDuyet: return "M.ResponseMessage.Common.DaDuyet";
                case CommonResponseMessage.M_ResponseMessage_Common_DaThoaiDuyet: return "M.ResponseMessage.Common.DaThoaiDuyet";
                case CommonResponseMessage.M_ResponseMessage_Common_DaTuChoiDuyet: return "M.ResponseMessage.Common.DaTuChoiDuyet";
                case CommonResponseMessage.M_ResponseMessage_Common_PhamViDuLieuKhongPhuHop: return "M.ResponseMessage.Common.PhamViDuLieuKhongPhuHop";

                case CommonResponseMessage.M_ResponseMessage_Common_NotExistCMND: return "M.ResponseMessage.Common.NotExistCMND";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistCMND: return "M.ResponseMessage.Common.ExistCMND";
                case CommonResponseMessage.M_ResponseMessage_Common_NotExistCMNDGiaDinh: return "M.ResponseMessage.Common.NotExistCMNDGiaDinh";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistCMNDGiaDinh: return "M.ResponseMessage.Common.ExistCMNDGiaDinh";
                case CommonResponseMessage.M_ResponseMessage_Common_NotExistCMNDNguoiThuaKe: return "M.ResponseMessage.Common.NotExistCMNDNguoiThuaKe";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistCMNDNguoiThuaKe: return "M.ResponseMessage.Common.ExistCMNDNguoiThuaKe";
                case CommonResponseMessage.M_ResponseMessage_Common_NotExistCMNDNguoiBaoLanh: return "M.ResponseMessage.Common.NotExistCMNDNguoiBaoLanh";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistCMNDNguoiBaoLanh: return "M.ResponseMessage.Common.ExistCMNDNguoiBaoLanh";

                case CommonResponseMessage.M_ResponseMessage_Common_NotExistDKKD: return "M.ResponseMessage.Common.NotExistDKKD";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistDKKD: return "M.ResponseMessage.Common.ExistDKKD";

                case CommonResponseMessage.M_ResponseMessage_Common_NotExistSoHoKhau: return "M.ResponseMessage.Common.NotExistSoHoKhau";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistSoHoKhau: return "M.ResponseMessage.Common.ExistSoHoKhau";
                case CommonResponseMessage.M_ResponseMessage_Common_NotExistSoHoKhauGiaDinh: return "M.ResponseMessage.Common.NotExistSoHoKhauGiaDinh";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistSoHoKhauGiaDinh: return "M.ResponseMessage.Common.ExistSoHoKhauGiaDinh";
                case CommonResponseMessage.M_ResponseMessage_Common_NotExistSoHoKhauNguoiThuaKe: return "M.ResponseMessage.Common.NotExistSoHoKhauNguoiThuaKe";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistSoHoKhauNguoiThuaKe: return "M.ResponseMessage.Common.ExistSoHoKhauNguoiThuaKe";
                case CommonResponseMessage.M_ResponseMessage_Common_NotExistSoHoKhauNguoiBaoLanh: return "M.ResponseMessage.Common.NotExistSoHoKhauNguoiBaoLanh";
                case CommonResponseMessage.M_ResponseMessage_Common_ExistSoHoKhauNguoiBaoLanh: return "M.ResponseMessage.Common.ExistSoHoKhauNguoiBaoLanh";
                default: return "";
            }
        }

        public enum UtilitesResponseMessage
        {
            M_ResponseMessage_Utilities_NotExistCMND,
            M_ResponseMessage_Utilities_ExistCMND,
            M_ResponseMessage_Utilities_NotExistCMNDGiaDinh,
            M_ResponseMessage_Utilities_ExistCMNDGiaDinh,
            M_ResponseMessage_Utilities_NotExistCMNDNguoiThuaKe,
            M_ResponseMessage_Utilities_ExistCMNDNguoiThuaKe,
            M_ResponseMessage_Utilities_NotExistCMNDNguoiBaoLanh,
            M_ResponseMessage_Utilities_ExistCMNDNguoiBaoLanh,

            M_ResponseMessage_Utilities_NotExistDKKD,
            M_ResponseMessage_Utilities_ExistDKKD,

            M_ResponseMessage_Utilities_NotExistSoHoKhau,
            M_ResponseMessage_Utilities_ExistSoHoKhau,
            M_ResponseMessage_Utilities_NotExistSoHoKhauGiaDinh,
            M_ResponseMessage_Utilities_ExistSoHoKhauGiaDinh,
            M_ResponseMessage_Utilities_NotExistSoHoKhauNguoiThuaKe,
            M_ResponseMessage_Utilities_ExistSoHoKhauNguoiThuaKe,
            M_ResponseMessage_Utilities_NotExistSoHoKhauNguoiBaoLanh,
            M_ResponseMessage_Utilities_ExistSoHoKhauNguoiBaoLanh
        };
        public static string layGiaTri(this UtilitesResponseMessage item)
        {
            switch (item)
            {
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistCMND: return "M.ResponseMessage.Utilities.NotExistCMND";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistCMND: return "M.ResponseMessage.Utilities.ExistCMND";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistCMNDGiaDinh: return "M.ResponseMessage.Utilities.NotExistCMNDGiaDinh";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistCMNDGiaDinh: return "M.ResponseMessage.Utilities.ExistCMNDGiaDinh";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistCMNDNguoiThuaKe: return "M.ResponseMessage.Utilities.NotExistCMNDNguoiThuaKe";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistCMNDNguoiThuaKe: return "M.ResponseMessage.Utilities.ExistCMNDNguoiThuaKe";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistCMNDNguoiBaoLanh: return "M.ResponseMessage.Utilities.NotExistCMNDNguoiBaoLanh";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistCMNDNguoiBaoLanh: return "M.ResponseMessage.Utilities.ExistCMNDNguoiBaoLanh";

                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistDKKD: return "M.ResponseMessage.Utilities.NotExistDKKD";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistDKKD: return "M.ResponseMessage.Utilities.ExistDKKD";

                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistSoHoKhau: return "M.ResponseMessage.Utilities.NotExistSoHoKhau";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistSoHoKhau: return "M.ResponseMessage.Utilities.ExistSoHoKhau";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistSoHoKhauGiaDinh: return "M.ResponseMessage.Utilities.NotExistSoHoKhauGiaDinh";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistSoHoKhauGiaDinh: return "M.ResponseMessage.Utilities.ExistSoHoKhauGiaDinh";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistSoHoKhauNguoiThuaKe: return "M.ResponseMessage.Utilities.NotExistSoHoKhauNguoiThuaKe";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistSoHoKhauNguoiThuaKe: return "M.ResponseMessage.Utilities.ExistSoHoKhauNguoiThuaKe";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_NotExistSoHoKhauNguoiBaoLanh: return "M.ResponseMessage.Utilities.NotExistSoHoKhauNguoiBaoLanh";
                case UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistSoHoKhauNguoiBaoLanh: return "M.ResponseMessage.Utilities.ExistSoHoKhauNguoiBaoLanh";
                default: return "";
            }
        }

        /// <summary>
        /// Message trả về từ Server cho Client khi login
        /// </summary>
        public enum LoginResponseMessage
        {
            M_ResponseMessage_Login_ThanhCong,
            M_ResponseMessage_Login_KhongThanhCong,
            M_ResponseMessage_Login_SaiThongTinDangNhap,
            M_ResponseMessage_Login_TaiKhoanBiKhoa,
            M_ResponseMessage_Login_TaiKhoanHetHan,
            M_ResponseMessage_Login_YeuCauDoiMatKhau,
            M_ResponseMessage_Login_YeuCauDoiMatKhauLanDau,
            M_ResponseMessage_Login_OverSession,
            M_ResponseMessage_Login_LoseSession,
            M_ResponseMessage_Login_TaiKhoanChuaDuocDuyet,
            M_ResponseMessage_Login_ChuaToiNgayHieuLuc,
            M_ResponseMessage_Login_TaiKhoanKhongDuocSuDung,
            M_ResponseMessage_Login_LoadUserRolesFailed,
            M_ResponseMessage_Login_LoadUserBranchesFailed,
            M_ResponseMessage_Login_LoadBusinessDateFailed,
            M_ResponseMessage_Login_LoadParametersFailed,
            M_ResponseMessage_Login_DefineSessionFailed,
            M_ResponseMessage_Login_DiaChiMacVaIpKhongHopLe,
            M_ResponseMessage_Login_DiaChiMacKhongHopLe,
            M_ResponseMessage_Login_DiaChiIpKhongHopLe,
            M_ResponseMessage_Login_DiaChiKhongHopLe,
            M_ResponseMessage_Login_LayNgonNguKhongThanhCong,
            M_ResponseMessage_Login_LayShowConfigKhongThanhCong
        };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// Giá trị trả về là key_language cho các thông báo
        /// </summary>
        /// <param name="loaiLich">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoginResponseMessage loginResponseMessage)
        {
            switch (loginResponseMessage)
            {
                case LoginResponseMessage.M_ResponseMessage_Login_ThanhCong: return "M.ResponseMessage.Login.ThanhCong";
                case LoginResponseMessage.M_ResponseMessage_Login_KhongThanhCong: return "M.ResponseMessage.Login.KhongThanhCong";
                case LoginResponseMessage.M_ResponseMessage_Login_SaiThongTinDangNhap: return "M.ResponseMessage.Login.SaiThongTinDangNhap";
                case LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanBiKhoa: return "M.ResponseMessage.Login.TaiKhoanBiKhoa";
                case LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanHetHan: return "M.ResponseMessage.Login.TaiKhoanHetHan";
                case LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhau: return "M.ResponseMessage.Login.YeuCauDoiMatKhau";
                case LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhauLanDau: return "M.ResponseMessage.Login.YeuCauDoiMatKhauLanDau";
                case LoginResponseMessage.M_ResponseMessage_Login_OverSession: return "M.ResponseMessage.Login.OverSession";
                case LoginResponseMessage.M_ResponseMessage_Login_LoseSession: return "M.ResponseMessage.Login.LoseSession";
                case LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanChuaDuocDuyet: return "M.ResponseMessage.Login.TaiKhoanChuaDuocDuyet";
                case LoginResponseMessage.M_ResponseMessage_Login_ChuaToiNgayHieuLuc: return "M.ResponseMessage.Login.ChuaToiNgayHieuLuc";
                case LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanKhongDuocSuDung: return "M.ResponseMessage.Login.TaiKhoanKhongDuocSuDung";
                case LoginResponseMessage.M_ResponseMessage_Login_LoadUserRolesFailed: return "M.ResponseMessage.Login.LoadUserRolesFailed";
                case LoginResponseMessage.M_ResponseMessage_Login_LoadUserBranchesFailed: return "M.ResponseMessage.Login.LoadUserBranchesFailed";
                case LoginResponseMessage.M_ResponseMessage_Login_LoadBusinessDateFailed: return "M.ResponseMessage.Login.LoadBusinessDateFailed";
                case LoginResponseMessage.M_ResponseMessage_Login_LoadParametersFailed: return "M.ResponseMessage.Login.LoadParametersFailed";
                case LoginResponseMessage.M_ResponseMessage_Login_DefineSessionFailed: return "M.ResponseMessage.Login.DefineSessionFailed";
                case LoginResponseMessage.M_ResponseMessage_Login_DiaChiMacVaIpKhongHopLe: return "M_ResponseMessage.Login.DiaChiMacVaIpKhongHopLe";
                case LoginResponseMessage.M_ResponseMessage_Login_DiaChiMacKhongHopLe: return "M_ResponseMessage.Login.DiaChiMacKhongHopLe";
                case LoginResponseMessage.M_ResponseMessage_Login_DiaChiIpKhongHopLe: return "M_ResponseMessage.Login.DiaChiIpKhongHopLe";
                case LoginResponseMessage.M_ResponseMessage_Login_DiaChiKhongHopLe: return "M_ResponseMessage.Login.DiaChiKhongHopLe";
                case LoginResponseMessage.M_ResponseMessage_Login_LayNgonNguKhongThanhCong: return "M_ResponseMessage.Login.LayNgonNguKhongThanhCong";
                case LoginResponseMessage.M_ResponseMessage_Login_LayShowConfigKhongThanhCong: return "M_ResponseMessage.Login.LayShowConfigKhongThanhCong";
                    
                default: return "";
            }
        }

        /// <summary>
        /// Message trả về từ Server cho Client khi quản lý danh mục
        /// </summary>
        public enum DanhMucResponseMessage
        {
            M_ResponseMessage_DanhMuc_ThanhCong,
            M_ResponseMessage_DanhMuc_KhongThanhCong,
            M_ResponseMessage_DanhMuc_KhongTonTaiDuLieu,
            M_ResponseMessage_DanhMuc_RangBuocDuLieu,
            M_ResponseMessage_DanhMuc_DaTonTaiDuLieu,
            M_ResponseMessage_DanhMuc_DmDonVi_KhongThanhCong,
            M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiDVIHoacHSC,
            M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiDVI,
            M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiHSC,
            M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiVPGD,
            M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiVDGD,
            M_ResponseMessage_DanhMuc_DmDonVi_KhongThietLapDuocNgayLamViec,
            M_ResponseMessage_DanhMuc_DmDonVi_KhongThietLapDuocThamSo,
            M_ResponseMessage_DanhMuc_DmDonVi_KhongXoaDuocDuLieuNgayLamViec,
            M_ResponseMessage_DanhMuc_DmDonVi_KhongXoaDuocDuLieuThamSo,
            M_ResponseMessage_DanhMuc_DmDonVi_KhongXoaDuocDuLieuDonVi,
            M_ResponseMessage_DanhMuc_DmNhom_DaTonTaiNhomCon,
            M_ResponseMessage_DanhMuc_DmNhom_DaTonTaiKhachHangThuocNhomCon,
        };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// Giá trị trả về là key_language cho các thông báo
        /// </summary>
        /// <param name="loaiLich">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this DanhMucResponseMessage danhMucResponseMessage)
        {
            switch (danhMucResponseMessage)
            {
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_ThanhCong: return "M.ResponseMessage.DanhMuc.ThanhCong";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_KhongThanhCong: return "M.ResponseMessage.DanhMuc.KhongThanhCong";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_KhongTonTaiDuLieu: return "M.ResponseMessage.DanhMuc.KhongTonTaiDuLieu";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_RangBuocDuLieu: return "M.ResponseMessage.DanhMuc.RangBuocDuLieu";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DaTonTaiDuLieu: return "M.ResponseMessage.DanhMuc.DaTonTaiDuLieu";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_KhongThanhCong: return "M.ResponseMessage.DanhMuc.DmDonVi.KhongThanhCong";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiDVIHoacHSC: return "M.ResponseMessage.DanhMuc.DmDonVi.DaTonTaiDVIHoacHSC";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiDVI: return "M.ResponseMessage.DanhMuc.DmDonVi.DaTonTaiDVI";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiHSC: return "M.ResponseMessage.DanhMuc.DmDonVi.DaTonTaiHSC";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiVPGD: return "M.ResponseMessage.DanhMuc.DmDonVi.DaTonTaiVPGD";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_DaTonTaiVDGD: return "M.ResponseMessage.DanhMuc.DmDonVi.DaTonTaiVGDG";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_KhongThietLapDuocNgayLamViec: return "M.ResponseMessage.DanhMuc.DmDonVi.KhongThietLapDuocNgayLamViec";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_KhongThietLapDuocThamSo: return "M.ResponseMessage.DanhMuc.DmDonVi.KhongThietLapDuocThamSo";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_KhongXoaDuocDuLieuNgayLamViec: return "M.ResponseMessage.DanhMuc.DmDonVi.KhongXoaDuocDuLieuNgayLamViec";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_KhongXoaDuocDuLieuThamSo: return "M.ResponseMessage.DanhMuc.DmDonVi.KhongXoaDuocDuLieuThamSo";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmDonVi_KhongXoaDuocDuLieuDonVi: return "M.ResponseMessage.DanhMuc.DmDonVi.KhongXoaDuocDuLieuDonVi";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmNhom_DaTonTaiNhomCon: return "M.ResponseMessage.DanhMuc.DmNhom.DaTonTaiNhomCon";
                case DanhMucResponseMessage.M_ResponseMessage_DanhMuc_DmNhom_DaTonTaiKhachHangThuocNhomCon: return "M.ResponseMessage.DanhMuc.DmNhom.DaTonTaiKhachHangThuocNhomCon";
                default: return "";
            }
        }

        public enum QuanTriHeThongResponseMessage
        {
            M_ResponseMessage_QuanTriHeThong_ThanhCong,
            M_ResponseMessage_QuanTriHeThong_KhongThanhCong,
            M_ResponseMessage_QuanTriHeThong_RangBuocDuLieu,
            M_ResponseMessage_QuanTriHeThong_NguoiDung_DaTonTai,
            M_ResponseMessage_QuanTriHeThong_NguoiDung_KhongTonTai,
            M_ResponseMessage_QuanTriHeThong_NhomNguoiDung_DaTonTai,
            M_ResponseMessage_QuanTriHeThong_NhomNguoiDung_KhongTonTai,
            M_ResponseMessage_QuanTriHeThong_NguoiDung_MatKhauCuKhongChinhXac,
            M_ResponseMessage_QuanTriHeThong_PhienBan_KiemTraKhongThanhCong,
            M_ResponseMessage_QuanTriHeThong_PhienBan_ThongTinKhongChinhXac
        };
        public static string layGiaTri(this QuanTriHeThongResponseMessage quanTriHeThongResponseMessage)
        {
            switch (quanTriHeThongResponseMessage)
            {
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong: return "M.ResponseMessage.QuanTriHeThong.ThanhCong";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong: return "M.ResponseMessage.QuanTriHeThong.KhongThanhCong";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_RangBuocDuLieu: return "M.ResponseMessage.QuanTriHeThong.RangBuocDuLieu";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_DaTonTai: return "M.ResponseMessage.QuanTriHeThong.NguoiDung.DaTonTai";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_KhongTonTai: return "M.ResponseMessage.QuanTriHeThong.NguoiDung.KhongTonTai";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NhomNguoiDung_DaTonTai: return "M.ResponseMessage.QuanTriHeThong.NhomNguoiDung.DaTonTai";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NhomNguoiDung_KhongTonTai: return "M.ResponseMessage.QuanTriHeThong.NhomNguoiDung.KhongTonTai";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_MatKhauCuKhongChinhXac: return "M.ResponseMessage.QuanTriHeThong.NguoiDung.MatKhauCuKhongChinhXac";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_KiemTraKhongThanhCong: return "M.ResponseMessage.QuanTriHeThong.PhienBan.KiemTraKhongThanhCong";
                case QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_ThongTinKhongChinhXac: return "M.ResponseMessage.QuanTriHeThong.PhienBan.ThongTinKhongChinhXac";
                default: return "";
            }
        }

        public enum BaoCaoResponseMessage
        {
            M_ResponseMessage_BaoCao_ThanhCong,
            M_ResponseMessage_BaoCao_KhongThanhCong,
            M_ResponseMessage_BaoCao_KhongTonTaiFileBaoCao,
            M_ResponseMessage_BaoCao_KhongCoDuLieuBaoCao,
            M_ResponseMessage_BaoCao_KhongLayDuocDuLieuGiaoDich,
            M_ResponseMessage_BaoCao_KhongLayDuocThongTinChungTu,
            M_ResponseMessage_BaoCao_KhongCoThongTinChungTu,
            M_ResponseMessage_BaoCao_KhongLayDuocThongTinSo,
            M_ResponseMessage_BaoCao_KhongCoThongTinSo,
            M_ResponseMessage_BaoCao_KhongLayDuocThongTinHachToan,
            M_ResponseMessage_BaoCao_KhongCoThongTinHachToan,
            M_ResponseMessage_BaoCao_LoiLayDuLieuBaoCao,
            M_ResponseMessage_BaoCao_LoiTaoDuLieuBaoCao,
            M_ResponseMessage_BaoCao_LoiTaoDuLieuChungTu
        };
        public static string layGiaTri(this BaoCaoResponseMessage quanTriHeThongResponseMessage)
        {
            switch (quanTriHeThongResponseMessage)
            {
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_ThanhCong: return "M.ResponseMessage.BaoCao.ThanhCong";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongThanhCong: return "M.ResponseMessage.BaoCao.KhongThanhCong";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongTonTaiFileBaoCao: return "M.ResponseMessage.BaoCao.KhongTonTaiFileBaoCao";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongCoDuLieuBaoCao: return "M.ResponseMessage.BaoCao.KhongCoDuLieuBaoCao";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongLayDuocDuLieuGiaoDich: return "M.ResponseMessage.BaoCao.KhongLayDuocDuLieuGiaoDich";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongLayDuocThongTinChungTu: return "M.ResponseMessage.BaoCao.KhongLayDuocThongTinChungTu";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongCoThongTinChungTu: return "M.ResponseMessage.BaoCao.KhongCoThongTinChungTu";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongLayDuocThongTinSo: return "M.ResponseMessage.BaoCao.KhongLayDuocThongTinSo";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongCoThongTinSo: return "M.ResponseMessage.BaoCao.KhongCoThongTinSo";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongLayDuocThongTinHachToan: return "M.ResponseMessage.BaoCao.KhongLayDuocThongTinHachToan";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_KhongCoThongTinHachToan: return "M.ResponseMessage.BaoCao.KhongCoThongTinHachToan";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_LoiLayDuLieuBaoCao: return "M.ResponseMessage.BaoCao.LoiLayDuLieuBaoCao";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_LoiTaoDuLieuBaoCao: return "M.ResponseMessage.BaoCao.LoiTaoDuLieuBaoCao";
                case BaoCaoResponseMessage.M_ResponseMessage_BaoCao_LoiTaoDuLieuChungTu: return "M.ResponseMessage.BaoCao.LoiTaoDuLieuChungTu";
                default: return "";
            }
        }

        /// <summary>
        /// Message trả về từ Server cho Client của LichLamViec
        /// </summary>
        public enum LichLamViecResponseMessage
        {
            M_Response_LichLamViec_LayDSNgayLamViec,
            M_Response_LichLamViec_LuuLich,
        };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// Giá trị trả về là key_language cho các thông báo
        /// </summary>
        /// <param name="loaiLich">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LichLamViecResponseMessage responseMessage)
        {
            switch (responseMessage)
            {
                case LichLamViecResponseMessage.M_Response_LichLamViec_LayDSNgayLamViec: return "M.Response.LichLamViec.LayDSNgayLamViec";
                case LichLamViecResponseMessage.M_Response_LichLamViec_LuuLich: return "M.Response.LichLamViec.LuuLich";
                default: return "";
            }
        }

        /// <summary>
        /// Các service được dùng trong hệ thống
        /// </summary>
        public enum SystemService
        {
            BaoCaoService,
            BaoHiemService,
            ChiTieuService,
            DanhMucService,
            EFBaoCaoService,
            HanMucService,
            HoSoTinDungService,
            HuyDongVonService,
            KeToanService,
            KhachHangService,
            KhaiThacDuLieuService,
            LaiSuatService,
            PhiService,
            NganQuyService,
            NhanSuService,
            PhongToaService,
            PopupService,
            QuanTriHeThongService,
            TaiSanDamBaoService,
            ThanhVienService,
            TinDungService,
            TinDungTTService,
            TruyVanService,
            TyGiaService,
            UtilitiesService,
            ZAMainAppService,
            ZATestAppService,
            TaiSanService,
            TinDungTDService,
            DeliveryService,
            JobService,
            ScheduleService,
            SupportService,
            WebProcessService,
            SMSService
        };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiLich">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this SystemService systemService)
        {
            switch (systemService)
            {
                case SystemService.BaoCaoService: return "BaoCaoService";
                case SystemService.EFBaoCaoService: return "EFBaoCaoService";
                case SystemService.BaoHiemService: return "BaoHiemService";
                case SystemService.ChiTieuService: return "ChiTieuService";
                case SystemService.DanhMucService: return "DanhMucService";
                case SystemService.HanMucService: return "HanMucService";
                case SystemService.HoSoTinDungService: return "HoSoTinDungService";
                case SystemService.HuyDongVonService: return "HuyDongVonService";
                case SystemService.KeToanService: return "KeToanService";
                case SystemService.KhachHangService: return "KhachHangService";
                case SystemService.KhaiThacDuLieuService: return "KhaiThacDuLieuService";
                case SystemService.LaiSuatService: return "LaiSuatService";
                case SystemService.PhiService: return "PhiService";
                case SystemService.NganQuyService: return "NganQuyService";
                case SystemService.NhanSuService: return "NhanSuService";
                case SystemService.PhongToaService: return "PhongToaService";
                case SystemService.PopupService: return "PopupService";
                case SystemService.QuanTriHeThongService: return "QuanTriHeThongService";
                case SystemService.TaiSanDamBaoService: return "TaiSanDamBaoService";
                case SystemService.ThanhVienService: return "ThanhVienService";
                case SystemService.TinDungService: return "TinDungService";
                case SystemService.TinDungTTService: return "TinDungTTService";                    
                case SystemService.TruyVanService: return "TruyVanService";
                case SystemService.TyGiaService: return "TyGiaService";
                case SystemService.UtilitiesService: return "UtilitiesService";
                case SystemService.ZAMainAppService: return "ZAMainAppService";
                case SystemService.ZATestAppService: return "ZATestAppService";
                case SystemService.TaiSanService: return "TaiSanService";
                case SystemService.TinDungTDService: return "TinDungTDService";
                case SystemService.DeliveryService: return "DeliveryService";
                case SystemService.JobService: return "JobService";
                case SystemService.ScheduleService: return "ScheduleService";
                case SystemService.SupportService: return "SupportService";
                case SystemService.WebProcessService: return "WebProcessService";
                case SystemService.SMSService: return "SMSService";
                default: return "";
            }
        }

        public static string getInterfaceServiceName(this SystemService systemService)
        {
            switch (systemService)
            {
                case SystemService.BaoCaoService: return "IBaoCaoService";
                case SystemService.BaoHiemService: return "IBaoHiemService";
                case SystemService.ChiTieuService: return "IChiTieuService";
                case SystemService.DanhMucService: return "IDanhMucService";
                case SystemService.HanMucService: return "IHanMucService";
                case SystemService.HoSoTinDungService: return "IHoSoTinDungService";
                case SystemService.HuyDongVonService: return "IHuyDongVonService";
                case SystemService.KeToanService: return "IKeToanService";
                case SystemService.KhachHangService: return "IKhachHangService";
                case SystemService.KhaiThacDuLieuService: return "IKhaiThacDuLieuService";
                case SystemService.LaiSuatService: return "ILaiSuatService";
                case SystemService.PhiService: return "IPhiService";
                case SystemService.NganQuyService: return "INganQuyService";
                case SystemService.NhanSuService: return "INhanSuService";
                case SystemService.PhongToaService: return "IPhongToaService";
                case SystemService.PopupService: return "IPopupService";
                case SystemService.QuanTriHeThongService: return "IQuanTriHeThongService";
                case SystemService.TaiSanDamBaoService: return "ITaiSanDamBaoService";
                case SystemService.ThanhVienService: return "IThanhVienService";
                case SystemService.TinDungService: return "ITinDungService";
                case SystemService.TinDungTTService: return "TinDungTTService";                    
                case SystemService.TruyVanService: return "ITruyVanService";
                case SystemService.TyGiaService: return "ITyGiaService";
                case SystemService.UtilitiesService: return "IUtilitiesService";
                case SystemService.ZAMainAppService: return "IZAMainAppService";
                case SystemService.ZATestAppService: return "IZATestAppService";
                case SystemService.TaiSanService: return "ITaiSanService";
                case SystemService.TinDungTDService: return "ITinDungTDService";
                case SystemService.DeliveryService: return "IDeliveryService";
                case SystemService.JobService: return "IJobService";
                case SystemService.ScheduleService: return "IScheduleService";
                default: return "";
            }
        }

        public static string getServiceName(this SystemService systemService)
        {
            switch (systemService)
            {
                case SystemService.BaoCaoService: return "BaoCaoService";
                case SystemService.BaoHiemService: return "BaoHiemService";
                case SystemService.ChiTieuService: return "ChiTieuService";
                case SystemService.DanhMucService: return "DanhMucService";
                case SystemService.HanMucService: return "HanMucService";
                case SystemService.HoSoTinDungService: return "HoSoTinDungService";
                case SystemService.HuyDongVonService: return "HuyDongVonService";
                case SystemService.KeToanService: return "KeToanService";
                case SystemService.KhachHangService: return "KhachHangService";
                case SystemService.KhaiThacDuLieuService: return "KhaiThacDuLieuService";
                case SystemService.LaiSuatService: return "LaiSuatService";
                case SystemService.PhiService: return "PhiService";
                case SystemService.NganQuyService: return "NganQuyService";
                case SystemService.NhanSuService: return "NhanSuService";
                case SystemService.PhongToaService: return "PhongToaService";
                case SystemService.PopupService: return "PopupService";
                case SystemService.QuanTriHeThongService: return "QuanTriHeThongService";
                case SystemService.TaiSanDamBaoService: return "TaiSanDamBaoService";
                case SystemService.ThanhVienService: return "ThanhVienService";
                case SystemService.TinDungService: return "TinDungService";
                case SystemService.TinDungTTService: return "TinDungTTService";                    
                case SystemService.TruyVanService: return "TruyVanService";
                case SystemService.TyGiaService: return "TyGiaService";
                case SystemService.UtilitiesService: return "UtilitiesService";
                case SystemService.ZAMainAppService: return "ZAMainAppService";
                case SystemService.ZATestAppService: return "ZATestAppService";
                case SystemService.TaiSanService: return "TaiSanService";
                case SystemService.TinDungTDService: return "TinDungTDService";
                case SystemService.DeliveryService: return "DeliveryService";
                case SystemService.JobService: return "JobService";
                case SystemService.ScheduleService: return "ScheduleService";
                case SystemService.SMSService: return "SMSService";
                default: return "";
            }
        }

        /// <summary>
        /// Kiểm tra toàn vẹn dữ liệu cho message/data từ Client
        /// </summary>
        [Flags]
        public enum ValidationLevel
        {
            SecurityKey = 0x0001,
            SessionId = 0x0002,
            UserSession = 0x0004,
            BusinessSession = 0x0008,
            All = SecurityKey | SessionId | UserSession | BusinessSession
        }

        /// <summary>
        /// Level lock dữ liệu
        /// </summary>
        [Flags]
        public enum LockDataLevel
        {
            Read = 0x0001,
            Modify = 0x0002,
            Delete = 0x0004,
            All = Read | Modify | Delete
        }

        /// <summary>
        /// Type lock dữ liệu
        /// </summary>
        [Flags]
        public enum LockDataType
        {
            User = 0x0001,
            Session = 0x0002,
            Application = 0x0004,
            All = User | Session | Application
        }

        /// <summary>
        /// OperationStatus
        /// </summary>
        public enum OperationStatus
        {
            Successful,
            Failed,
            Pending,
            NoResponse,
            LoseSession,
            InvalidOrExpiredOperationSession,
            InvalidOrExpiredUserSession,
            InvalidWorkingDay
        };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiLich">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this OperationStatus operationStatus)
        {
            switch (operationStatus)
            {
                case OperationStatus.Successful: return "Successful";
                case OperationStatus.Failed: return "Failed";
                case OperationStatus.Pending: return "Pending";
                case OperationStatus.NoResponse: return "NoResponse";
                case OperationStatus.LoseSession: return "LoseSession";
                case OperationStatus.InvalidOrExpiredOperationSession: return "InvalidOrExpiredOperationSession";
                case OperationStatus.InvalidOrExpiredUserSession: return "InvalidOrExpiredUserSession";
                case OperationStatus.InvalidWorkingDay: return "InvalidWorkingDay";
                default: return "";
            }
        }

        /// <summary>
        /// Lấy ngôn ngữ cho OperationStatus
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string layNgonNgu(this OperationStatus item)
        {
            switch (item)
            {
                case OperationStatus.Successful: return "M.DungChung.Result.ThanhCong";
                case OperationStatus.Failed: return "M.DungChung.Result.KhongThanhCong";
                case OperationStatus.Pending: return "M.DungChung.Result.ChuaXuLy";
                default: return "";
            }
        }

        public enum LoaiThamSoBaoCao { LANG, FORMAT, GUI, SQL, GUISQL, GUIPARAM };
        public static string layGiaTri(this LoaiThamSoBaoCao loaiThamSoBaoCao)
        {
            switch (loaiThamSoBaoCao)
            {
                case LoaiThamSoBaoCao.LANG: return "LANG";
                case LoaiThamSoBaoCao.FORMAT: return "FORMAT";
                case LoaiThamSoBaoCao.GUI: return "GUI";
                case LoaiThamSoBaoCao.SQL: return "SQL";
                case LoaiThamSoBaoCao.GUISQL: return "GUISQL";
                case LoaiThamSoBaoCao.GUIPARAM: return "GUIPARAM";
                default: return "";
            }
        }

        public enum LoaiNgonNguBaoCao { vi_VN, en_US };
        public static string layGiaTri(this LoaiNgonNguBaoCao loaiNgonNguBaoCao)
        {
            switch (loaiNgonNguBaoCao)
            {
                case LoaiNgonNguBaoCao.vi_VN: return "vi-VN";
                case LoaiNgonNguBaoCao.en_US: return "en-US";
                default: return "";
            }
        }

        public enum LoaiDinhDangBaoCao { PDF, EXCEL, WORD, TEXT };
        public static string layGiaTri(this LoaiDinhDangBaoCao loaiDinhDangBaoCao)
        {
            switch (loaiDinhDangBaoCao)
            {
                case LoaiDinhDangBaoCao.PDF: return "PDF";
                case LoaiDinhDangBaoCao.EXCEL: return "EXCEL";
                case LoaiDinhDangBaoCao.WORD: return "WORD";
                case LoaiDinhDangBaoCao.TEXT: return "TEXT";
                default: return "";
            }
        }

        public enum LoaiMauBaoCao { RPT, EXCEL, JASPER, TEXT, WORD };
        public static string layGiaTri(this LoaiMauBaoCao loaiMauBaoCao)
        {
            switch (loaiMauBaoCao)
            {
                case LoaiMauBaoCao.RPT: return "RPT";
                case LoaiMauBaoCao.EXCEL: return "EXCEL";
                case LoaiMauBaoCao.JASPER: return "JASPER";
                case LoaiMauBaoCao.TEXT: return "TEXT";
                case LoaiMauBaoCao.WORD: return "WORD";
                default: return "";
            }
        }

        /// <summary>
        /// Loại khách hàng
        /// </summary>
        public enum LoaiKhachHang
        {
            THANH_VIEN,
            CA_NHAN,
            DOANH_NGHIEP,
            TCTD,
            KHAC
        }

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiKhachHang">enum LoaiKhachHang</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoaiKhachHang loaiKhachHang)
        {
            switch (loaiKhachHang)
            {
                case LoaiKhachHang.THANH_VIEN: return "TVIEN";
                case LoaiKhachHang.CA_NHAN: return "CNHAN";
                case LoaiKhachHang.DOANH_NGHIEP: return "DNGHIEP";
                case LoaiKhachHang.TCTD: return "TCTD";
                case LoaiKhachHang.KHAC: return "KHAC";
                default: return "";
            }
        }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public enum LoaiGiayTo
        {
            CHUNG_MINH_ND,
            HO_CHIEU,
            BANG_LAI_XE,
            GP_DKKD,
            GIAY_TO_KHAC
        }

        public static string layGiaTri(this LoaiGiayTo loaiGiayTo)
        {
            switch (loaiGiayTo)
            {
                case LoaiGiayTo.CHUNG_MINH_ND: return "CHUNG_MINH_ND";
                case LoaiGiayTo.HO_CHIEU: return "HO_CHIEU";
                case LoaiGiayTo.BANG_LAI_XE: return "BANG_LAI_XE";
                case LoaiGiayTo.GP_DKKD: return "GP_DKKD";
                case LoaiGiayTo.GIAY_TO_KHAC: return "GIAY_TO_KHAC";
                default: return "";
            }
        }

        /// <summary>
        /// Các đơn vị chiều dài
        /// </summary>
        public enum UnitWidth
        {
            Pixel,
            Star,
            Percent
        }

        public static string layGiaTri(this UnitWidth width)
        {
            switch (width)
            {
                case UnitWidth.Pixel: return "Pixel";
                case UnitWidth.Star: return "Star";
                case UnitWidth.Percent: return "Percent";
                default: return "";
            }
        }

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public enum LoaiDoiTuong
        {
            KHACH_HANG,
            SAN_PHAM_TIN_DUNG,
            HDTD_THONG_THUONG,
            KUOC_THONG_THUONG,
            HDTD_VI_MO,
            KUOC_VI_MO,
            SAN_PHAM_TIET_KIEM,
            SO_TIET_KIEM,
            TAI_KHOAN,
            GIAO_DICH,
            LAI_SUAT,
            TY_GIA,
            NGUOI_SU_DUNG,
            DON_VI,
            HAN_MUC,
            TSDB,
            HDTC,
            PHAN_LOAI_TAI_KHOAN
        }

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">enum LoaiDoiTuong</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoaiDoiTuong loaiDoiTuong)
        {
            switch (loaiDoiTuong)
            {
                case LoaiDoiTuong.KHACH_HANG: return "KHACH_HANG";
                case LoaiDoiTuong.SAN_PHAM_TIN_DUNG: return "SAN_PHAM_TIN_DUNG";
                case LoaiDoiTuong.HDTD_THONG_THUONG: return "HDTD_THONG_THUONG";
                case LoaiDoiTuong.KUOC_THONG_THUONG: return "KUOC_THONG_THUONG";
                case LoaiDoiTuong.HDTD_VI_MO: return "HDTD_VI_MO";
                case LoaiDoiTuong.KUOC_VI_MO: return "KUOC_VI_MO";
                case LoaiDoiTuong.SAN_PHAM_TIET_KIEM: return "SAN_PHAM_TIET_KIEM";
                case LoaiDoiTuong.SO_TIET_KIEM: return "SO_TIET_KIEM";
                case LoaiDoiTuong.TAI_KHOAN: return "TAI_KHOAN";
                case LoaiDoiTuong.GIAO_DICH: return "GIAO_DICH";
                case LoaiDoiTuong.LAI_SUAT: return "LAI_SUAT";
                case LoaiDoiTuong.TY_GIA: return "TY_GIA";
                case LoaiDoiTuong.NGUOI_SU_DUNG: return "NGUOI_SU_DUNG";
                case LoaiDoiTuong.DON_VI: return "DON_VI";
                case LoaiDoiTuong.TSDB: return "TSDB";
                case LoaiDoiTuong.HDTC: return "HDTC";
                case LoaiDoiTuong.PHAN_LOAI_TAI_KHOAN: return "PHAN_LOAI_TAI_KHOAN";
                default: return "";
            }
        }

        /// <summary>
        /// Message trả về từ Server cho Client khi xử lý nghiệp vụ
        /// </summary>
        public enum NghiepVuResponseMessage
        {
            //Hệ thống
            M_ResponseMessage_HeThong_DangGiaoDich, //Hệ thống đang giao dịch
            M_ResponseMessage_HeThong_TamNgungGiaoDich, //Hệ thống tạm ngừng giao dịch
            M_ResponseMessage_HeThong_NgungGiaoDich, //Hệ thống ngừng giao dịch

            //Dùng chung
            M_ResponseMessage_DungChung_ThanhCong,
            M_ResponseMessage_DungChung_KhongThanhCong,
            M_ResponseMessage_DungChung_LoiKhongXacDinh,
            M_ResponseMessage_DungChung_KhongTonTaiDichVuNghiepVu,
            M_ResponseMessage_DungChung_KhongTonTaiChucNangXuLy,
            M_ResponseMessage_DungChung_DaDuocSuDung, //Đã được sử dụng không thể xóa
            M_ResponseMessage_DungChung_KhongTonTai, //Không tồn tại
            M_ResponseMessage_DungChung_KhongThuocDonVi, //Không thuộc đơn vị
            M_ResponseMessage_DungChung_KhongThuocPhongGiaoDich, //Không thuộc phòng giao dịch

            //Hành động
            M_ResponseMessage_HanhDong_KhongDuDieuKienDeLuuTam, //Không đủ điều kiện để lưu tạm
            M_ResponseMessage_HanhDong_ThemThanhCong, //Thêm thành công
            M_ResponseMessage_HanhDong_ThemKhongThanhCong, //Thêm không thành công
            M_ResponseMessage_HanhDong_SuaThanhCong, //Sửa thành công
            M_ResponseMessage_HanhDong_SuaKhongThanhCong, //Sửa không thành công
            M_ResponseMessage_HanhDong_XoaThanhCong, //Xóa thành công
            M_ResponseMessage_HanhDong_XoaKhongThanhCong, //Xóa không thành công
            M_ResponseMessage_HanhDong_DuyetThanhCong, //Duyệt thành công
            M_ResponseMessage_HanhDong_DuyetKhongThanhCong, //Duyệt không thành công
            M_ResponseMessage_HanhDong_ThoaiDuyetThanhCong, //Thoái duyệt thành công
            M_ResponseMessage_HanhDong_ThoaiDuyetKhongThanhCong, //Thoái duyệt không thành công
            M_ResponseMessage_HanhDong_TuChoiDuyetThanhCong, //Từ chối duyệt thành công
            M_ResponseMessage_HanhDong_TuChoiDuyetKhongThanhCong, //Từ chối duyệt không thành công
            M_ResponseMessage_HanhDong_XuLyThanhCong, //Xử lý thành công
            M_ResponseMessage_HanhDong_XuLyKhongThanhCong, //Xử lý không thành công
            M_ResponseMessage_HanhDong_LuuTamThanhCong, //Lưu tạm thành công
            M_ResponseMessage_HanhDong_LuuTamKhongThanhCong, //Lưu tạm không thành công
            M_ResponseMessage_HanhDong_TrinhDuyetThanhCong, //Trình duyệt thành công
            M_ResponseMessage_HanhDong_TrinhDuyetKhongThanhCong, //Trình duyệt không thành công
            M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong, //Lấy dữ liệu không thành công

            //Mã
            M_ResponseMessage_Ma_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_Ma_DaTonTai, //Đã tồn tại 
            M_ResponseMessage_Ma_KhongTaoDuoc, //Không tạo được

            //Dữ liệu
            M_ResponseMessage_DuLieu_KhongDuocPhepThayDoi, //Dữ liệu không được phép thay đổi
            M_ResponseMessage_DuLieu_DaDuocSuDungKhongDuocXoa, //Dữ liệu đã được sử dụng không được xóa
            M_ResponseMessage_DuLieu_DaDuocSuDungKhongDuocSua, //Dữ liệu đã được sử dụng không được sửa

            //Hình thức giao dịch
            M_ResponseMessage_HinhThucGiaoDich_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_HinhThucGiaoDich_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_HinhThucGiaoDich_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_HinhThucGiaoDich_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Chỉ thị đáo hạn
            M_ResponseMessage_ChiThiDaoHan_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_ChiThiDaoHan_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_ChiThiDaoHan_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_ChiThiDaoHan_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Loại giao dịch
            M_ResponseMessage_LoaiGiaoDich_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_LoaiGiaoDich_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_LoaiGiaoDich_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_LoaiGiaoDich_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Mã đối tượng
            M_ResponseMessage_MaDoiTuong_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_MaDoiTuong_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_MaDoiTuong_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_MaDoiTuong_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Mã đối tượng
            M_ResponseMessage_MucDichVay_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_MucDichVay_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_MucDichVay_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_MucDichVay_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Loại chứng từ
            M_ResponseMessage_LoaiChungTu_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_LoaiChungTu_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_LoaiChungTu_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_LoaiChungTu_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Trạng thái nghiệp vụ
            M_ResponseMessage_TrangThaiNghiepVu_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TrangThaiNghiepVu_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TrangThaiNghiepVu_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TrangThaiNghiepVu_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_TrangThaiNghiepVu_KhongHopLe, //Ko hợp lệ

            //Khu vực
            M_ResponseMessage_KhuVuc_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_KhuVuc_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_KhuVuc_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_KhuVuc_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Cụm
            M_ResponseMessage_Cum_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_Cum_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_Cum_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_Cum_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Nhóm
            M_ResponseMessage_Nhom_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_Nhom_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_Nhom_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_Nhom_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Địa bàn
            M_ResponseMessage_DiaBan_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_DiaBan_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_DiaBan_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_DiaBan_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tỉnh thành phố
            M_ResponseMessage_TinhTP_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TinhTP_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TinhTP_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TinhTP_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tiền tệ
            M_ResponseMessage_TienTe_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TienTe_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TienTe_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TienTe_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Biểu phí
            M_ResponseMessage_BieuPhi_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_BieuPhi_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_BieuPhi_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_BieuPhi_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_BieuPhi_HetHieuLuc, //hết hiệu lực
            M_ResponseMessage_BieuPhi_ConHieuLuc, //Còn hiệu lực

            //Đơn vị
            M_ResponseMessage_DonVi_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_DonVi_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_DonVi_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_DonVi_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Khách hàng
            M_ResponseMessage_KhachHang_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_KhachHang_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_KhachHang_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_KhachHang_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_KhachHang_KhongThuocDonVi, //khách hàng không thuộc đơn vị
            M_ResponseMessage_KhachHang_ThuocDonVi, //khách hàng thuộc đơn vị
            M_ResponseMessage_KhachHang_LaThanhVien, //khách hàng là thành viên
            M_ResponseMessage_KhachHang_KhongLaThanhVien, //khách hàng không là thành viên
            M_ResponseMessage_KhachHang_CoTuoiKhongHopLe, //khách hàng có tuổi không hợp lệ
            M_ResponseMessage_KhachHang_CoTuoiHopLe, //khách hàng có tuổi hợp lệ
            M_ResponseMessage_KhachHang_HetHieuLuc, //khách hàng hết hiệu lực
            M_ResponseMessage_KhachHang_ConHieuLuc, //khách hàng còn hiệu lực
            M_ResponseMessage_KhachHang_HoKhauKhongThuocDiaPhuong, //Hộ khẩu thường trú của khách hàng không thuộc địa phương
            M_ResponseMessage_KhachHang_HoKhauThuocDiaPhuong, //Hộ khẩu thường trú của khách hàng thuộc địa phương
            M_ResponseMessage_KhachHang_SoKyNoTKQDChuaDuTieuChuan, //Số kỳ nộp tiết kiệm quy định của khách hàng chưa đủ tiêu chuẩn
            M_ResponseMessage_KhachHang_SoKyNoTKQDDuTieuChuan, //Số kỳ nộp tiết kiệm quy định của khách hàng đủ tiêu chuẩn
            M_ResponseMessage_KhachHang_SoKyNoTKQDDuKhongLayDuoc, //không lấy được Số kỳ nộp tiết kiệm quy định của khách hàng
            M_ResponseMessage_KhachHang_KhongDangCoHDTD, //Khách hàng không đang có hợp đồng tín dụng
            M_ResponseMessage_KhachHang_DangCoHDTD, //Khách hàng đang có hợp đồng tín dụng
            M_ResponseMessage_KhachHang_KhongDangDongTrachNhiem, //Khách hàng không đang đồng trách nhiệm với hợp đồng khác
            M_ResponseMessage_KhachHang_DangDongTrachNhiem, //Khách hàng đang đồng trách nhiệm với hợp đồng khác
            M_ResponseMessage_KhachHang_KhongDuocPhepTrong, //Mã khách hàng không được phép null hoặc trống
            M_ResponseMessage_KhachHang_LaChuTaiKhoan, //khách hàng là chủ tài khoản
            M_ResponseMessage_KhachHang_KhongLaChuTaiKhoan, //khách hàng không là chủ tài khoản
            M_ResponseMessage_KhachHang_ThuocDanhSachDCSH, //khách hàng thuộc danh sách đồng chủ sở hữu
            M_ResponseMessage_KhachHang_KhongHopLe, // khách hàng không hợp lệ
            M_ResponseMessage_KhachHang_ConDuNo, // khách hàng còn dư nợ
            M_ResponseMessage_KhachHang_ConSoDuTietKiem,  // khách hàng còn số dư tiết kiệm
            M_ResponseMessage_KhachHang_ThemDuLieuAnhKhongThanhCong,  // thêm dữ liệu ảnh không thành công
            M_ResponseMessage_KhachHang_SoCMNDDaTonTai,  // Số chứng minh nhân dân đã tồn tại
            M_ResponseMessage_KhachHang_SoHoKhauDaTonTai,  // Số chứng minh nhân dân đã tồn tại
            M_ResponseMessage_KhachHang_SoLuongKHDaDuTrongNhom,  // thêm dữ liệu ảnh không thành công
            M_ResponseMessage_KhachHang_KoTaoDuocMaKhachHang,  // Không tạo được mã khách hàng
            M_ResponseMessage_KhachHang_KhachHangKoThuocPGD,  // Khách hàng ko thuộc kiểm soát của phòng giao dịch
            M_ResponseMessage_KhachHang_DuyetThanhCong,  // Duyệt khách hàng thành công
            M_ResponseMessage_KhachHang_DuyetKhongThanhCong,  // Duyệt khách hàng không thành công
            M_ResponseMessage_KhachHang_SoCMNDKhongHopLe,  // Số chứng minh nhân dân không hợp lệ
            M_ResponseMessage_KhachHang_ConKhoanVayCungHoKhau,  // Con khoan vay cung ho khau
            M_ResponseMessage_KhachHang_SoDienThoaiDaTonTai,  // Số điện thoại đã tồn tại
            M_ResponseMessage_KhachHang_SoDiDongDaTonTai,  // Số di động đã tồn tại


            //Nhóm nợ
            M_ResponseMessage_NhomNo_HopLe, //Hợp lệ
            M_ResponseMessage_NhomNo_KhongHopLe, //Không Hợp lệ
            M_ResponseMessage_NhomNo_LonHonNhomNoHienTai, //Nhóm nợ mới lớn hơn nhóm nợ hiện  tại
            M_ResponseMessage_NhomNo_BangNhomNoHienTai, //Nhóm nợ mới bằng nhóm nợ hiện  tại
            M_ResponseMessage_NhomNo_NhoHonNhomNoHienTai, //Nhóm nợ mới nhỏ hơn nhóm nợ hiện  tại

            //Đồng chủ sở hữu
            M_ResponseMessage_DongChuSoHuu_KhongTonTai, //Không tồn tại nhân sự
            M_ResponseMessage_DongChuSoHuu_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_DongChuSoHuu_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_DongChuSoHuu_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_DongChuSoHuu_XoaKhongThanhCong, //Xóa không thành công
            M_ResponseMessage_DongChuSoHuu_ThemKhongThanhCong, //Thêm không thành công
            M_ResponseMessage_DongChuSoHuu_SuaKhongThanhCong, //Sửa không thành công
            M_ResponseMessage_DongChuSoHuu_SuaThanhCong, //Sửa thành công

            //Nhân sự
            M_ResponseMessage_NhanSu_KhongTonTai, //Không tồn tại nhân sự
            M_ResponseMessage_NhanSu_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_NhanSu_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_NhanSu_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_NhanSu_KhongThuocDonViTacNghiep,/// không thuộc đơn vị tác nghiệp
            M_ResponseMessage_NhanSu_ThuocDonViTacNghiep,/// thuộc đơn vị tác nghiệp

            //Nhóm Sản phẩm huy động vốn
            M_ResponseMessage_NhomSanPhamHuyDongVon_KhongHopLe,//Không hợp lệ
            M_ResponseMessage_NhomSanPhamHuyDongVon_HopLe,//Hợp lệ
            M_ResponseMessage_NhomSanPhamHuyDongVon_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_NhomSanPhamHuyDongVon_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_NhomSanPhamHuyDongVon_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_NhomSanPhamHuyDongVon_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch,

            //Sản phẩm huy động vốn
            M_ResponseMessage_SanPhamHuyDongVon_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_SanPhamHuyDongVon_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_SanPhamHuyDongVon_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch,
            M_ResponseMessage_SanPhamHuyDongVon_HetHieuLuc, //hết hiệu lực
            M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepLapLichTraLai, //Không được phép lập lịch trả lãi
            M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepLapLichGuiGop, //Không được phép lập lịch gửi góp
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemQuyDinh, //Không thuộc nhóm sp Tiết kiệm quy định
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemKhongKyHan,//Không thuộc nhóm sp Tiết kiệm tự nguyện không kỳ hạn
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiDinhKy,//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn trả lãi định kỳ
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiSau,//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn trả lãi sau
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiTruoc,//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn trả lãi trước
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanGuiGop,//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn gửi góp
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHan,//Không thuộc nhóm sp Tiền gửi có kỳ hạn
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTienGuiThanhToan,//Không thuộc nhóm sp Tiền gửi thanh toán
            M_ResponseMessage_SanPhamHuyDongVon_KhongDuocLapLichTraLai, //Không được lập lịch trả lãi
            M_ResponseMessage_SanPhamHuyDongVon_DuocLapLichTraLai, //Được lập lịch trả lãi
            M_ResponseMessage_SanPhamHuyDongVon_KhongDuocLapLichTraGop, //Không được lập lịch trả góp
            M_ResponseMessage_SanPhamHuyDongVon_DuocLapLichTraGop, //Được lập lịch trả góp
            M_ResponseMessage_SanPhamHuyDongVon_KhongTatToanTruocHan, //Không được tất toán trước hạn
            M_ResponseMessage_SanPhamHuyDongVon_CoTatToanTruocHan, //Được tất toán trước hạn
            M_ResponseMessage_SanPhamHuyDongVon_KhongThuocKiemSoatCuaDonVi, //Sản phẩm không thuộc kiểm soát của đơn vị

            //Lãi suất
            M_ResponseMessage_MaLaiSuat_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_MaLaiSuat_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_MaLaiSuat_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_MaLaiSuat_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_MaLaiSuat_HetHieuLuc, //hết hiệu lực
            M_ResponseMessage_MaLaiSuat_KhongHopLe, //Mã lãi suất không hợp lệ
            M_ResponseMessage_MaLaiSuat_HopLe, //Mã lãi suất hợp lệ
            M_ResponseMessage_MaLaiSuat_KhongThuocPhanHeNghiepVu, //Mã lãi suất không thuộc phân hệ nghiệp vụ
            M_ResponseMessage_MaLaiSuat_ThuocPhanHeNghiepVu, //Mã lãi suất thuộc phân hệ nghiệp vụ
            M_ResponseMessage_DVTinhLaiSuat_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_DVTinhLaiSuat_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_DVTinhLaiSuat_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_DVTinhLaiSuat_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tỷ lệ
            M_ResponseMessage_TyLe_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_TyLe_HopLe, //hợp lệ

            //Hệ số
            M_ResponseMessage_HeSo_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_HeSo_HopLe, //hợp lệ

            //Tần suất
            M_ResponseMessage_TanSuat_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_TanSuat_HopLe, //hợp lệ

            //Số kỳ hạn
            M_ResponseMessage_SoKyHan_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_SoKyHan_HopLe, //hợp lệ

            //Số tháng
            M_ResponseMessage_SoThang_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_SoThang_HopLe, //hợp lệ

            //Số ngày
            M_ResponseMessage_SoNgay_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_SoNgay_HopLe, //hợp lệ

            //Tiền lãi
            M_ResponseMessage_TienLai_TinhThanhCong, //tính không thành công
            M_ResponseMessage_TienLai_TinhKhongThanhCong, //Tính thành công


            //Số tiền lãi tháng
            M_ResponseMessage_TienLaiThang_KhongHopLe, //không hợp lệ
            M_ResponseMessage_TienLaiThang_HopLe, //hợp lệ
            M_ResponseMessage_TienLaiNgay_KhongHopLe, //không hợp lệ
            M_ResponseMessage_TienLaiNgay_HopLe, //không hợp lệ

            //Số tiền phong tỏa
            M_ResponseMessage_SoTienPhongToa_KhongHopLe, //không hợp lệ
            M_ResponseMessage_SoTienPhongToa_HopLe, //hợp lệ

            //Số tiền kỳ
            M_ResponseMessage_SoTienKy_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_SoTienKy_HopLe, //hợp lệ
            M_ResponseMessage_SoTienKy_KhongLayDuoc, //Không lấy được

            //Số tiền
            M_ResponseMessage_SoTien_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_SoTien_HopLe, //hợp lệ
            M_ResponseMessage_SoTien_BangTongLaiThangVaLaiNgay, //Số tiền rút lãi Bằng tổng lãi tháng và lãi ngày
            M_ResponseMessage_SoTien_KhongBangTongLaiThangVaLaiNgay, //Số tiền rút lãi không Bằng tổng lãi tháng và lãi ngày
            M_ResponseMessage_SoTien_BangTongRutGocVaRutLai, //Số tiền giao dịch bằng tổng số tiền rút gốc và số tiền rút lãi
            M_ResponseMessage_SoTien_KhongBangTongRutGocVaRutLai, //Số tiền giao dịch không bằng tổng số tiền rút gốc và số tiền rút lãi
            M_ResponseMessage_SoTien_KhongBangSoDu, //Số tiền rút gốc không bằng số dư
            M_ResponseMessage_SoTien_BangSoDu, //Số tiền rút gốc bằng số dư
            M_ResponseMessage_SoTien_TraLaiNhoHonBangLaiTinhDuoc, //Số tiền trả lãi nhỏ hơn bằng số tiền lãi tính được
            M_ResponseMessage_SoTien_TraLaiLonHonLaiTinhDuoc, //Số tiền trả lãi lớn hơn bằng số tiền lãi tính được
            M_ResponseMessage_SoTien_PhanBoTonTai, //Số tiền phân bổ tồn tài 
            M_ResponseMessage_SoTien_PhanBoKhongTonTai, //Số tiền phân bổ không tồn tài 
            M_ResponseMessage_SoTien_PhanBoLonHonTongTienTraLai, //Số tiền phân bổ lớn hơn tổng tiền trả lãi
            M_ResponseMessage_SoTien_PhanBoNhoHonBangTongTienTraLai, //Số tiền phân bổ nhỏ hơn bằng tổng tiền trả lãi
            M_ResponseMessage_SoTien_LaiNhapGocLonHonLaiDenNgay, //Số tiền lãi nhập gốc lớn hơn lãi tính đến ngày
            M_ResponseMessage_SoTien_LaiNhapGocNhoHonBangLaiDenNgay, //Số tiền lãi nhập gốc nhỏ hơn bằng lãi tính đến ngày
            M_ResponseMessage_SoTien_NhoHonSoToiThieu, //Số tiền nhỏ hơn số tối thiểu
            M_ResponseMessage_SoTien_LonHonSoToiDa, //Số tiền lớn hơn số tối đa
            M_ResponseMessage_SoTien_HoanDuThuKhongHopLe, // Số tiền hoàn dự thu không hợp lệ
            M_ResponseMessage_SoTien_SoTKBBNhoHon10PhanTramDuNoConLai, // Số dư của sổ TKBB nhỏ hơn 10% dư nợ còn lại của khoản vay

            //Số dư
            M_ResponseMessage_SoDu_ConLaiNhoHonSoDuQuyDinhTrongSanPham, // Số dư còn lại nhỏ hơn số dư tối thiểu quy định ở sản phầm

            //Lãi suất
            M_ResponseMessage_LaiSuat_NhoHonKhong, //Lãi suất nhỏ hơn không
            M_ResponseMessage_LaiSuat_KhongHopLe, //Không hợp lê
            M_ResponseMessage_LaiSuat_HopLe, //hợp lê
            M_ResponseMessage_LaiSuat_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_LaiSuat_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_LaiSuat_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_LaiSuat_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_LaiSuat_DonViTinhKhongHopLe, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_LaiSuat_NgayApDungKhongHopLe, //Ngày áp dụng không hợp lệ
            M_ResponseMessage_LaiSuat_KhongThucQuanLyCuaDonVi, //Lãi suất không thuộc quản lý của đơn vị đăng nhập

            //Lãi suất quá hạn
            M_ResponseMessage_LaiSuatQuanHan_KhongHopLe, //Không hợp lê
            M_ResponseMessage_LaiSuatQuanHan_NhoHonLaiQuaHanToiDa, //Nhỏ hơn bằng lãi quá hạn tối đa
            M_ResponseMessage_LaiSuatQuanHan_LonHonLaiQuaHanToiDa, //Lớn hơn lãi quá hạn tối đa

            //Lãi suất cơ cấu lại kỳ hạn hạn trả nợ
            M_ResponseMessage_LaiSuatCoCau_KhongHopLe, //Không hợp lê

            //Cơ sở tính lãi
            M_ResponseMessage_CoSoTinhLai_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_CoSoTinhLai_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_CoSoTinhLai_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_CoSoTinhLai_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_CoSoTinhLai_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_CoSoTinhLai_MauSoKhongHopLe, //mẫu số không hợp lệ


            //Loại giao dịch phong tỏa
            M_ResponseMessage_LoaiGDPhongToa_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_LoaiGDPhongToa_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_LoaiGDPhongToa_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_LoaiGDPhongToa_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Phương thức phong tỏa
            M_ResponseMessage_PhuongThucPhongToa_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_PhuongThucPhongToa_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_PhuongThucPhongToa_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_PhuongThucPhongToa_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Loại nguồn vốn giải ngân
            M_ResponseMessage_LoaiNguonVon_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_LoaiNguonVon_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_LoaiNguonVon_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_LoaiNguonVon_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Nguyên nhân quá hạn
            M_ResponseMessage_NguyenNhanQuaHan_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_NguyenNhanQuaHan_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_NguyenNhanQuaHan_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_NguyenNhanQuaHan_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Phong tỏa - giải tỏa
            M_ResponseMessage_PhongToa_LonHonSoDu, //Số tiền phong tỏa lớn hơn số dư
            M_ResponseMessage_PhongToa_NhonHonBangSoDu, //Số tiền phong tỏa nhỏ hơn hoặc bằng số dư

            //Hạn mức
            M_ResponseMessage_HanMuc_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_HanMuc_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_HanMuc_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_HanMuc_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_HanMuc_HetHieuLuc, //hết hiệu lực
            M_ResponseMessage_HanMuc_HieuLuc, //Đang hiệu lực
            M_ResponseMessage_HanMuc_KhongThuocKhachHang, //không thuộc khách hàng
            M_ResponseMessage_HanMuc_ThuocKhachHang, //thuộc khách hàng
            M_ResponseMessage_HanMuc_VuotHanMucChoPhep, // vượt quá hạn mức được phép

            //Loại tiền
            M_ResponseMessage_LoaiTien_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_LoaiTien_TonTai, //tồn tại 
            M_ResponseMessage_LoaiTien_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_LoaiTien_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_LoaiTien_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_LoaiTien_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tính chất Mã loại tài khoản (nội bảng - ngoại bảng)
            M_ResponseMessage_MaLoaiTaiKhoan_TonTai, //tồn tại 
            M_ResponseMessage_MaLoaiTaiKhoan_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_MaLoaiTaiKhoan_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_MaLoaiTaiKhoan_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_MaLoaiTaiKhoan_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tính chất Mã loại khách hàng nội bộ- khách hàng
            M_ResponseMessage_MaLoaiKHangNBo_TonTai, //tồn tại 
            M_ResponseMessage_MaLoaiKHangNBo_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_MaLoaiKHangNBo_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_MaLoaiKHangNBo_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_MaLoaiKHangNBo_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tính chất Mã loại thu nhap - chi phi
            M_ResponseMessage_MaLoaiTNhapCPhi_TonTai, //tồn tại 
            M_ResponseMessage_MaLoaiTNhapCPhi_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_MaLoaiTNhapCPhi_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_MaLoaiTNhapCPhi_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_MaLoaiTNhapCPhi_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tính chất số dư
            M_ResponseMessage_TinhChatSoDu_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_TinhChatSoDu_TonTai, //tồn tại 
            M_ResponseMessage_TinhChatSoDu_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TinhChatSoDu_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TinhChatSoDu_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TinhChatSoDu_DuocPhepGiaoDich,//tồn tại, đang hoạt động, và được phép giao dịch

            //Phân loại tài khoản
            M_ResponseMessage_PhanLoaiTaiKhoan_DauVaoKhongHopLe, //Đầu vào không hợp lệ 
            M_ResponseMessage_PhanLoaiTaiKhoan_TonTai, //tồn tại 
            M_ResponseMessage_PhanLoaiTaiKhoan_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_PhanLoaiTaiKhoan_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_PhanLoaiTaiKhoan_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_PhanLoaiTaiKhoan_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_PhanLoaiTaiKhoan_TenKhongHopLe, //Tên phân loại tài khoản không hợp lệ
            M_ResponseMessage_PhanLoaiTaiKhoan_MaKhongHopLe, //Mã phân loại tài khoản không hợp lệ
            M_ResponseMessage_PhanLoaiTaiKhoan_MaChaDaCoTaiKhoanChiTiet, //Mã phân loại cha đã có tài khoản chi tiết
            M_ResponseMessage_PhanLoaiTaiKhoan_KhongThuocQuanLyCuaDonVi, //Mã phân loại không thuộc quản lý của đơn vị
            M_ResponseMessage_PhanLoaiTaiKhoan_KhongNhoNhat, //Mã phân loại không thuộc quản lý của đơn vị

            //Tài khoản
            M_ResponseMessage_TaiKhoan_TenKhongHopLe, //Tên tài khoản không hợp lệ
            M_ResponseMessage_TaiKhoan_DauVaoKhongHopLe, //Đầu vào không hợp lệ
            M_ResponseMessage_TaiKhoan_KhongHopLe, //Tài khoản không hợp lệ
            M_ResponseMessage_TaiKhoan_TonTai, //Tài khoản đang tồn tai
            M_ResponseMessage_TaiKhoan_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TaiKhoan_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TaiKhoan_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TaiKhoan_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNoiBo, //không phải là tài khoản nội bộ
            M_ResponseMessage_TaiKhoan_LaTaiKhoanNoiBo, //là tài khoản nội bộ
            M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanKhachHang, //không phải là tài khoản khách hàng 
            M_ResponseMessage_TaiKhoan_LaTaiKhoanKhachHang, //là tài khoản khách hàng 
            M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNoiBang, //không phải là tài khoản nội bảng
            M_ResponseMessage_TaiKhoan_LaTaiKhoanNoiBang, //là tài khoản nội bảng
            M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNgoaiBang, //không phải là tài khoản ngoại bảng
            M_ResponseMessage_TaiKhoan_LaTaiKhoanNgoaiBang, //là tài khoản ngoại bảng
            M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanThuNhap, //không phải là tài khoản thu nhập
            M_ResponseMessage_TaiKhoan_LaTaiKhoanThuNhap, //là tài khoản thu nhập
            M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanChiPhi, //không phải là tài khoản chi phí
            M_ResponseMessage_TaiKhoan_LaTaiKhoanChiPhi, //là tài khoản chi phí
            M_ResponseMessage_TaiKhoan_KhongDuSoDuKhaDung, //Không đủ số dư khả dụng
            M_ResponseMessage_TaiKhoan_KhongDuSoDu, //Không đủ số dư 
            M_ResponseMessage_TaiKhoan_ChuaDong, //Tài khoản chưa đóng
            M_ResponseMessage_TaiKhoan_DangDong, //Tài khoản đang đóng
            M_ResponseMessage_TaiKhoan_TongNoBangTongCo, //Tổng nợ bằng tổng có
            M_ResponseMessage_TaiKhoan_TongNoLonHonTongCo, //Tổng nợ lớn hơn tổng có
            M_ResponseMessage_TaiKhoan_TongNoNhoHonTongCo, //Tổng nợ nhỏ hơn tổng có
            M_ResponseMessage_TaiKhoan_KhongThuocKhachHang, //Không thuộc khách hàng
            M_ResponseMessage_TaiKhoan_ThuocKhachHang, //Thuộc khách hàng
            M_ResponseMessage_TaiKhoan_KhongThuocDonVi, //Không thuộc đơn vị
            M_ResponseMessage_TaiKhoan_ThuocDonVi, //Thuộc đơn vị
            M_ResponseMessage_TaiKhoan_NgayPhongToaKhongHopLe, //Không hợp lệ
            M_ResponseMessage_TaiKhoan_NgayPhongToaHopLe, //hợp lệ
            M_ResponseMessage_TaiKhoan_NgayDongKhongHopLe, //Không hợp lệ
            M_ResponseMessage_TaiKhoan_NgayDongHopLe, //hợp lệ
            M_ResponseMessage_TaiKhoan_NgayMoKhongHopLe, //Không hợp lệ
            M_ResponseMessage_TaiKhoan_NgayMoHopLe, //hợp lệ
            M_ResponseMessage_TaiKhoan_ChuaDangKy, //Chưa đăng ký
            M_ResponseMessage_TaiKhoan_PhanLoaiKhongHopLe, //Phân loại tài khoản không hợp lệ
            M_ResponseMessage_TaiKhoan_KhongThuocQuanLyCuaDonVi, //Tài khoản không thuộc quản lý của đơn vị

            //Tài khoản khách hàng
            M_ResponseMessage_TaiKhoanKhachHang_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TaiKhoanKhachHang_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TaiKhoanKhachHang_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TaiKhoanKhachHang_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_TaiKhoanKhachHang_DangBiDong, //Đang bị đóng
            M_ResponseMessage_TaiKhoanKhachHang_DangBiPhongToa, //Đang bị phong tỏa
            M_ResponseMessage_TaiKhoanKhachHang_KhongDuSoDu, //không đủ sổ dư
            M_ResponseMessage_TaiKhoanKhachHang_KhongDuSoDuKhaDung, //không đủ sổ dư khả dụng
            M_ResponseMessage_TaiKhoanKhachHang_KhongThuocCSH, //không thuộc khách hàng, khách hàng không là chủ sở hữu

            //Tài khoản nội bộ
            M_ResponseMessage_TaiKhoanNoiBo_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TaiKhoanNoiBo_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TaiKhoanNoiBo_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TaiKhoanNoiBo_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_TaiKhoanNoiBo_DangBiDong, //Đang bị đóng
            M_ResponseMessage_TaiKhoanNoiBo_DangBiPhongToa, //Đang bị phong toa
            M_ResponseMessage_TaiKhoanNoiBo_KhongDuSoDu, //không đủ số dư
            M_ResponseMessage_TaiKhoanNoiBo_KhongDuSoDuKhaDung, //không đủ sổ dư khả dụng
            M_ResponseMessage_TaiKhoanNoiBo_KhongThuocCSH, //không thuộc đơn vị, khách hàng không là chủ sở hữu

            //Kế hoạch
            M_ResponseMessage_KeHoach_ThemKhongThanhCong, // Thêm không thành công
            M_ResponseMessage_KeHoach_ThemThanhCong, //Thêm thành công
            M_ResponseMessage_KeHoach_SuaKhongThanhCong, //Sửa không thành công
            M_ResponseMessage_KeHoach_SuaThanhCong, //Sửa thành công
            M_ResponseMessage_KeHoach_XoaKhongThanhCong, //Xóa không thành công
            M_ResponseMessage_KeHoach_XoaThanhCong, //Xóa thành công

            //Từ ngày, đến ngày
            M_ResponseMessage_TuDen_TuNgayLonHonDenNgay, //Từ ngày lớn hơn đến ngày
            M_ResponseMessage_TuDen_TuNgayNhoHonBangDenNgay, //Từ ngày nhỏ hơn bằng đến ngày

            //Ngày dự kiến phát vốn
            M_ResponseMessage_NgayDuKienPhatVon_KhongHopLe, //Ngày lập không hợp lệ
            M_ResponseMessage_NgayDuKienPhatVon_HopLe, //Ngày lập hợp lệ
            M_ResponseMessage_NgayDuKienPhatVon_BangNgayGiaoDich, //Ngày lập bằng ngày giao dịch
            M_ResponseMessage_NgayDuKienPhatVon_NhoHonNgayGiaoDich, //Ngày lập nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayDuKienPhatVon_LonHonNgayGiaoDich, //Ngày lập lớn hơn ngày giao dịch

            //Ngày làm việc
            M_ResponseMessage_NgayLamViec_HopLe, //Ngày làm việc hợp lệ
            M_ResponseMessage_NgayLamViec_KhongHopLe, //Ngày làm việc không hợp lệ
            M_ResponseMessage_NgayLamViec_KhongKhongTonTai, //Ngày làm việc không tồn tại

            //Gia hạn nợ
            M_ResponseMessage_GiaHan_GocKhongHopLe, //Gia han gốc không hợp lệ
            M_ResponseMessage_GiaHan_GocHopLe, //Gia han gốc hợp lệ
            M_ResponseMessage_GiaHan_LaiKhongHopLe, //Gia han lãi không hợp lệ
            M_ResponseMessage_GiaHan_LaiHopLe, //Gia han lãi hợp lệ

            //Ngày gia hạn
            M_ResponseMessage_NgayGiaHan_KhongHopLe, //Ngày gia han không hợp lệ
            M_ResponseMessage_NgayGiaHan_BangNgayGiaoDich, //Ngày gia han bằng ngày giao dịch
            M_ResponseMessage_NgayGiaHan_NhoHonNgayGiaoDich, //Ngày gia han nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayGiaHan_LonHonNgayGiaoDich, //Ngày gia han lớn hơn ngày giao dịch

            //Ngày thu
            M_ResponseMessage_NgayThu_KhongHopLe, //Ngày thu không hợp lệ
            M_ResponseMessage_NgayThu_BangNgayGiaoDich, //Ngày thu bằng ngày giao dịch
            M_ResponseMessage_NgayThu_NhoHonNgayGiaoDich, //Ngày thu nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayThu_LonHonNgayGiaoDich, //Ngày thu lớn hơn ngày giao dịch

            //Ngày trả
            M_ResponseMessage_NgayTra_LonHonNgayDangKy, //Ngày trả phải từ 15 ngày trờ lên kể từ ngày đăng ký

            //Ngày áp dụng
            M_ResponseMessage_NgayApDung_KhongHopLe, //Ngày lập không hợp lệ
            M_ResponseMessage_NgayApDung_BangNgayGiaoDich, //Ngày lập bằng ngày giao dịch
            M_ResponseMessage_NgayApDung_NhoHonNgayGiaoDich, //Ngày lập nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayApDung_LonHonNgayGiaoDich, //Ngày lập lớn hơn ngày giao dịch

            //Ngày hợp đồng
            M_ResponseMessage_NgayHopDong_KhongHopLe, //Ngày hợp đồng không hợp lệ
            M_ResponseMessage_NgayHopDong_BangNgayGiaoDich, //Ngày hợp đồng bằng ngày giao dịch
            M_ResponseMessage_NgayHopDong_NhoHonNgayGiaoDich, //Ngày hợp đồng nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayHopDong_LonHonNgayGiaoDich, //Ngày hợp đồng lớn hơn ngày giao dịch
            M_ResponseMessage_NgayHopDong_NhoHonNgayKhangThamGia, //Ngày hợp đồng nhỏ hơn ngày khách hàng tham gia

            //Ngày giao dịch
            M_ResponseMessage_NgayGiaoDich_KhongHopLe, //Ngày lập không hợp lệ
            M_ResponseMessage_NgayGiaoDich_BangNgayGiaoDich, //Ngày lập bằng ngày làm việc
            M_ResponseMessage_NgayGiaoDich_NhoHonNgayGiaoDich, //Ngày lập nhỏ hơn ngày làm việc
            M_ResponseMessage_NgayGiaoDich_LonHonNgayGiaoDich, //Ngày lập lớn hơn ngày làm việc

            //Ngày mở sổ
            M_ResponseMessage_NgayMoSo_KhongHopLe, // không hợp lệ
            M_ResponseMessage_NgayMoSo_BangNgayGiaoDich, //bằng ngày giao dịch
            M_ResponseMessage_NgayMoSo_NhoHonNgayGiaoDich, //nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayMoSo_LonHonNgayGiaoDich, //lớn hơn ngày giao dịch
            M_ResponseMessage_NgayMoSo_KhachNgayGiaoDich, //khác ngày giao dịch


            //Ngày lập
            M_ResponseMessage_NgayLap_KhongHopLe, //Ngày lập không hợp lệ
            M_ResponseMessage_NgayLap_BangNgayGiaoDich, //Ngày lập bằng ngày giao dịch
            M_ResponseMessage_NgayLap_NhoHonNgayGiaoDich, //Ngày lập nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayLap_LonHonNgayGiaoDich, //Ngày lập lớn hơn ngày giao dịch

            //Ngày cập nhật
            M_ResponseMessage_NgayCapNhat_KhongHopLe, //Ngày cập nhật không hợp lệ
            M_ResponseMessage_NgayCapNhat_BangNgayGiaoDich, //Ngày cập nhật bằng ngày giao dịch
            M_ResponseMessage_NgayCapNhat_NhoHonNgayGiaoDich, //Ngày cập nhật nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayCapNhat_LonHonNgayGiaoDich, //Ngày cập nhật lớn hơn ngày giao dịch

            //Ngày giải ngân
            M_ResponseMessage_NgayGiaiNgan_HopLe,//hợp lệ
            M_ResponseMessage_NgayGiaiNgan_KhongHopLe,//không hợp lệ
            M_ResponseMessage_NgayGiaiNgan_BangNgayGiaoDich,//bằng ngày giao dịch
            M_ResponseMessage_NgayGiaiNgan_NhoHonNgayGiaoDich,//nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayGiaiNgan_LonHonNgayGiaoDich,//lớn hơn ngày giao dịch

            //Dự thu đến ngày
            M_ResponseMessage_DuThuDenNgay_HopLe,//hợp lệ
            M_ResponseMessage_DuThuDenNgay_KhongHopLe,//không hợp lệ
            M_ResponseMessage_DuThuDenNgay_BangNgayGiaoDich,//bằng ngày giao dịch
            M_ResponseMessage_DuThuDenNgay_NhoHonNgayGiaoDich,//nhỏ hơn ngày giao dịch
            M_ResponseMessage_DuThuDenNgay_LonHonNgayGiaoDich,//lớn hơn ngày giao dịch

            //Ngày tính lãi
            M_ResponseMessage_NgayTinhLai_HopLe,//hợp lệ
            M_ResponseMessage_NgayTinhLai_KhongHopLe,//không hợp lệ
            M_ResponseMessage_NgayTinhLai_BangNgayGiaoDich,//bằng ngày giao dịch
            M_ResponseMessage_NgayTinhLai_NhoHonNgayGiaoDich,//nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayTinhLai_LonHonNgayGiaoDich,//lớn hơn ngày giao dịch

            //Ngày dự chi
            M_ResponseMessage_NgayDuChi_HopLe,//hợp lệ
            M_ResponseMessage_NgayDuChi_KhongHopLe,//không hợp lệ
            M_ResponseMessage_NgayDuChi_BangNgayGiaoDich,//bằng ngày giao dịch
            M_ResponseMessage_NgayDuChi_NhoHonNgayGiaoDich,//nhỏ hơn ngày giao dịch
            M_ResponseMessage_NgayDuChi_LonHonNgayGiaoDich,//lớn hơn ngày giao dịch

            //Người lập
            M_ResponseMessage_NguoiLap_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_NguoiLap_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_NguoiLap_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_NguoiLap_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Người cập nhật
            M_ResponseMessage_NguoiCapNhat_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_NguoiCapNhat_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_NguoiCapNhat_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_NguoiCapNhat_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Người sử dụng
            M_ResponseMessage_NguoiSuDung_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_NguoiSuDung_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_NguoiSuDung_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_NguoiSuDung_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_NguoiSuDung_ChuaDuocPhepThemMoi, //chưa được phép thêm mới
            M_ResponseMessage_NguoiSuDung_ChuaDuocPhepSua, //chưa được phép sửa
            M_ResponseMessage_NguoiSuDung_ChuaDuocPhepXoa, //chưa được phép xóa
            M_ResponseMessage_NguoiSuDung_ChuaDuocPhepDuyet, //chưa được phép duyệt
            M_ResponseMessage_NguoiSuDung_ChuaDuocPhepThoaiDuyet, //chưa được phép thoái duyệt
            M_ResponseMessage_NguoiSuDung_ChuaDuocPhepTuChoiDuyet, //chưa được phép từ chối duyệt

            //Sổ tiết kiệm
            M_ResponseMessage_SoTietKiem_TonTai, //tồn tại 
            M_ResponseMessage_SoTietKiem_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_SoTietKiem_KhongHopLe, //Không hợp lệ
            M_ResponseMessage_SoTietKiem_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_SoTietKiem_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_SoTietKiem_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_SoTietKiem_NgayApDungKhongHopLe, //ngày áp dụng không hợp lệ
            M_ResponseMessage_SoTietKiem_NgayMoSoKhongHopLe, //ngày mở sổ không hợp lệ
            M_ResponseMessage_SoTietKiem_NgayDaoHanKhongHopLe, //ngày đáo hạn không hợp lệ
            M_ResponseMessage_SoTietKiem_TienMatKhongHopLe, //tiền mặt không hợp lệ
            M_ResponseMessage_SoTietKiem_SoTienBangTienMatKhongHopLe, //số tiền bằng tiền mặt không hợp lệ
            M_ResponseMessage_SoTietKiem_SoTienBangChuyenKhoanKhongHopLe, //số tiền bằng chuyển khoản không hợp lệ
            M_ResponseMessage_SoTietKiem_NhoHonSoToiThieu, //số tiền mở sổ nhỏ hơn số tiền tối thiểu được quy định theo sản phẩm
            M_ResponseMessage_SoTietKiem_LonHonSoToiDa, //số tiền mở sổ lớn hơn số tiền tối đa được quy định theo sản phẩm
            M_ResponseMessage_SoTietKiem_DaTatToan, //số tiền mở hoặc tài khoản đã tất toán hoặc đóng
            M_ResponseMessage_SoTietKiem_ChuaTatToan, //số tiền mở hoặc tài khoản chưa tất toán hoặc đóng
            M_ResponseMessage_SoTietKiem_KhongThuocDonVi, //Sổ tiết kiệm không thuộc đơn vị
            M_ResponseMessage_SoTietKiem_KhongThuocPhongGiaoDich, //số tiết kiệm không thuộc phòng giao dịch
            M_ResponseMessage_SoTietKiem_ThuocDonVi, //số tiền mở thuộc đơn vị
            M_ResponseMessage_SoTietKiem_SoTienRutGocNhoHonKhong, //Nhỏ hơn không
            M_ResponseMessage_SoTietKiem_SoTienRutGocLonHonSoDuKhaDung, //Lớn hơn số dư khả dụng
            M_ResponseMessage_SoTietKiem_SoTienRutGocNhoHonBangSoDuKhaDung, //lớn hơn 0 và nhỏ hơn bằng số dư khả dụng
            M_ResponseMessage_SoTietKiem_XoaThanhCong, //Xóa thành công
            M_ResponseMessage_SoTietKiem_DuyetThanhCong, //Duyệt thành công
            M_ResponseMessage_SoTietKiem_ThoaiDuyetThanhCong, //Thoái duyệt thành công
            M_ResponseMessage_SoTietKiem_TuChoiDuyetThanhCong, //Từ chối duyệt thành công
            M_ResponseMessage_SoTietKiem_XoaKhongThanhCong, //Xóa không thành công
            M_ResponseMessage_SoTietKiem_DuyetKhongThanhCong, //Duyệt không thành công
            M_ResponseMessage_SoTietKiem_ThoaiDuyetKhongThanhCong, //Thoái duyệt không thành công
            M_ResponseMessage_SoTietKiem_TuChoiDuyetKhongThanhCong, //Từ chối duyệt không thành công
            M_ResponseMessage_SoTietKiem_SuaThanhCong,//Sửa thành công
            M_ResponseMessage_SoTietKiem_SuaKhongThanhCong,//Sửa không thành công
            M_ResponseMessage_SoTietKiem_ThemThanhCong,//Thêm thành công
            M_ResponseMessage_SoTietKiem_ThemKhongThanhCong,//Thêm không thành công
            M_ResponseMessage_SoTietKiem_TaoThongTinLichSuKhongThanhCong,//Tạo thông tin lịch sử ko thành công
            M_ResponseMessage_SoTietKiem_SaoLuuKhongThanhCong,//Sao lưu ko thành công
            M_ResponseMessage_SoTietKiem_XoaLichSuKhongThanhCong, //Xoa khong thanh công
            M_ResponseMessage_SoTietKiem_XoaLichSuThanhCong, //Xoa thanh công
            M_ResponseMessage_SoTietKiem_ThemLichSuKhongThanhCong, //Thêm khong thanh công
            M_ResponseMessage_SoTietKiem_ThemLichSuThanhCong, //Thêm thanh công
            M_ResponseMessage_SoTietKiem_ChuaDenHanTatToan, //Chưa đến hạn tất toán
            M_ResponseMessage_SoTietKiem_TinhDuChiKhongThanhCong, //Tính dự chi không thành công
            M_ResponseMessage_SoTietKiem_TinhLaiSTKKKHKhongThanhCong, //Tính lãi sổ tiết kiệm không kỳ hạn không thành công
            M_ResponseMessage_SoTietKiem_TinhLaiSTKQDKhongThanhCong, //Tính lãi sổ tiết kiệm quy định không thành công
            M_ResponseMessage_SoTietKiem_TinhLaiSTKCKHTHKhongThanhCong, //Tính lãi sổ tiết kiệm quy định không thành công
            M_ResponseMessage_SoTietKiem_DaThucHienLNGDenHetThang, //Đã thực hiện lãi nhập gốc đến hết tháng
            M_ResponseMessage_SoTietKiem_ChuaDenNgayNhanLai, //Chưa đến ngày nhận lãi
            M_ResponseMessage_SoTietKiem_DaThucHienLNGHoacTraLaiDenHienTai, //Đã thực hiện lãi nhập gốc hoặc trả lãi đến hiện tại
            M_ResponseMessage_SoTietKiem_DaNhanLai, //Sổ tiết kiệm đã nhận lãi
            M_ResponseMessage_SoTietKiem_KoDuocTatToanTruocHan, //Không được tất toán trước hạn
            M_ResponseMessage_SoTietKiem_KoLayDuocLaiSuatTruocHan, //Không lấy được lãi suất trước hạn
            M_ResponseMessage_SoTietKiem_DaDuChiDenHienTai, //Đã dự chi đến hiện tại
            M_ResponseMessage_SoTietKiem_ChuaDenNgayLNG, //Chưa đến ngày lãi nhập gốc
            M_ResponseMessage_SoTietKiem_ChuaThucHienNgayLNG, //Chưa thực hiện lãi nhập gốc
            M_ResponseMessage_SoTietKiem_DangTonTaiGiaoDichChoDuyet, //Đang tồn tại giao dịch chờ duyệt
            M_ResponseMessage_SoTietKiem_DangTonTaiGiaoDichChuaDuyet, //Đang tồn tại giao dịch chưa duyệt
            M_ResponseMessage_SoTietKiem_DaTonTaiMotSoKhacDangHoatDong, //Đã tồn tại một sổ khác của khách hàng đang hoạt động
            M_ResponseMessage_SoTietKiem_KhongDuocPhepTatToanKhiConSoDu, //Không được phép tất toán khi còn số dư
            M_ResponseMessage_SoTietKiem_ThanhVienChuaDatDuSoVongVayVon, //Thành viên chưa đạt đủ số vòng vay vốn

            //Huy động vốn - Mở sổ tiết kiệm quy định
            M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_ChuaDuocKhoiTao, //Chưa được khởi tạo
            M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayLamViecKhongHopLe, //Ngày làm việc không hợp lệ
            M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayMoSoKhongHopLe, //Ngày mở sổ không hợp lệ
            M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayDaoHanKhongHopLe, //Ngày đáo hạn không hợp lệ
            M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_HinhThucGiaoDichKhongTonTai, //Hình thức giao dịch không tồn tại
            M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_KhongDuocPhepSuaThongTin, //Không được phép sửa thông tin

            //Sổ tiết kiệm - Tài khoản CA
            M_ResponseMessage_HuyDongVon_TaiKhoanCA_ThemThanhCong,
            M_ResponseMessage_HuyDongVon_TaiKhoanCA_ThemKhongThanhCong,
            M_ResponseMessage_HuyDongVon_TaiKhoanCA_DuyetThanhCong,
            M_ResponseMessage_HuyDongVon_TaiKhoanCA_DuyetKhongThanhCong,

            //Lãi quá hạn
            M_ResponseMessage_LaiQuaHan_HopLe, //Lãi quá hạn hợp lệ, không vượt quy định của hệ thống
            M_ResponseMessage_LaiQuaHan_khongHopLe, //Lãi quá hạn không hợp lệ, vượt quy định của hệ thống

            //Sản phẩm tín dụng 
            M_ResponseMessage_SanPhamTinDung_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_SanPhamTinDung_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_SanPhamTinDung_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_SanPhamTinDung_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Hợp đồng thế chấp
            M_ResponseMessage_HDTC_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_HDTC_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_HDTC_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_HDTC_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Tài sản đảm bảo
            M_ResponseMessage_TSDB_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TSDB_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TSDB_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TSDB_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_TSDB_ThuocKhachHang, //Tài sản thuộc khách hàng
            M_ResponseMessage_TSDB_KhongThuocKhachHang, //Tài sản không thuộc khách hàng
            M_ResponseMessage_TSDB_VuotQuaSoTienDamBao, //Vượt quá số tiền có thể sử dụng để đảm bảo
            M_ResponseMessage_TSDB_KhongVuotQuaSoTienDamBao, //Không Vượt quá số tiền có thể sử dụng để đảm bảo

            //Hợp đồng tín dụng vi mô
            M_ResponseMessage_HDTDVM_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_HDTDVM_TonTai, //tồn tại 
            M_ResponseMessage_HDTDVM_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_HDTDVM_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_HDTDVM_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Khế ước vi mô
            M_ResponseMessage_KUOCVM_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_KUOCVM_TonTai, //tồn tại 
            M_ResponseMessage_KUOCVM_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_KUOCVM_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_KUOCVM_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_KUOCVM_ConDuNo, //Còn dư nợ
            M_ResponseMessage_KUOCVM_HetDuNo, //Hết dư nợ
            M_ResponseMessage_KUOCVM_ThuocKhachHang, //Thuộc khách hàng
            M_ResponseMessage_KUOCVM_KhongThuocKhachHang, //Không Thuộc khách hàng
            M_ResponseMessage_KUOCVM_SoTienGiaiNganKhongHopLe, //Số tiền giải ngân không hợp lệ
            M_ResponseMessage_KUOCVM_SoTienGiaiNganHopLe, //Số tiền giải ngân hợp lệ
            M_ResponseMessage_KUOCVM_VuotQuaHanMucChoPhep, //Vượt quá hạn mức cho phép
            M_ResponseMessage_KUOCVM_ChuaVuotQuaHanMucChoPhep, //Vượt quá hạn mức cho phép
            M_ResponseMessage_KUOCVM_SoTienGocNhoHonSoTienGiaiNgan, //Số tiền gốc nhỏ hơn số tiền giải ngân
            M_ResponseMessage_KUOCVM_SoTienGocBangSoTienGiaiNgan, //Số tiền gốc bằng số tiền giải ngân
            M_ResponseMessage_KUOCVM_SoTienGocLonHonSoTienGiaiNgan, //Số tiền gốc lơn hơn số tiền giải ngân
            M_ResponseMessage_KUOCVM_TonTaiKWQuaHan, //Tồn tại khế ước quá hạn
            M_ResponseMessage_KUOCVM_KhongTonTaiKWQuaHan, //Không Tồn tại khế ước quá hạn
            M_ResponseMessage_KUOCVM_ChuaDamBaoTyLeTraNo, //Chưa đảm bảo tỷ lệ trả nợ
            M_ResponseMessage_KUOCVM_DamBaoTyLeTraNo, //đảm bảo tỷ lệ trả nợ
            M_ResponseMessage_KUOCVM_NgayBDTraGocViPhamQuyDinh, //Ngày bắt đầu trả gốc vi phạm quy định
            M_ResponseMessage_KUOCVM_NgayBDTraGocKhongViPhamQuyDinh, //Ngày bắt đầu trả gốc không vi phạm quy định
            M_ResponseMessage_KUOCVM_NgayBDTraLaiViPhamQuyDinh, //Ngày bắt đầu trả lãi vi phạm quy định
            M_ResponseMessage_KUOCVM_NgayBDTraLaiKhongViPhamQuyDinh, //Ngày bắt đầu trả lãi không vi phạm quy định
            M_ResponseMessage_KUOCVM_DKyDGiaLSuatHopLe, //Định kỳ đánh giá lại lãi suất hợp lệ
            M_ResponseMessage_KUOCVM_DKyDGiaLSuatKhongHopLe, //Định kỳ đánh giá lại lãi suất không hợp lệ
            M_ResponseMessage_KUOCVM_ChuaDuocGiaiNgan, //Khế ước chưa được giải ngân
            M_ResponseMessage_KUOCVM_DaDuocGiaiNgan, //Khế ước đã được giải ngân
            M_ResponseMessage_KUOCVM_NhomNo1, //Nhóm nợ 1
            M_ResponseMessage_KUOCVM_NhomNo2, //Nhóm nợ 2
            M_ResponseMessage_KUOCVM_NhomNo3, //Nhóm nợ 3
            M_ResponseMessage_KUOCVM_NhomNo4, //Nhóm nợ 4
            M_ResponseMessage_KUOCVM_NhomNo5, //Nhóm nợ 5
            M_ResponseMessage_KUOCVM_KhongThuocNhomNo5, //Khong thuoc nhóm nợ 5
            M_ResponseMessage_KUOCVM_DaTatToan, //Đã tất toán
            M_ResponseMessage_KUOCVM_ChuaTatToan, //Chưa tất toán
            M_ResponseMessage_KUOCVM_CuaKhachHangLienQuanChuaTatToan, //KW của khách hàng liên quan Chưa tất toán
            M_ResponseMessage_KUOCVM_CuaKhachHangLienQuanDaTatToan, //KW của khách hàng liên quan đã tất toán
            M_ResponseMessage_KUOCVM_QuaHan, //KW quá hạn
            M_ResponseMessage_KUOCVM_KhongQuaHan, //KW không quá hạn
            M_ResponseMessage_KUOCVM_ThoiGianVayKhongHopLe, //KW thời gian vay không hợp lệ
            M_ResponseMessage_KUOCVM_ThoiGianVayHopLe, //KW thời gian vay hợp lệ
            M_ResponseMessage_KUOCVM_SoTienTKQDKhongDu, //Số tiền TKQD không đủ
            M_ResponseMessage_KUOCVM_VayHaiKheUocCungMotSanPham, //Vay hai khế ước cùng một sản phẩm
            M_ResponseMessage_KUOCVM_DaDuocPhanBoTrongThang,
            M_ResponseMessage_KUOCVM_KyThuKhongHopLe,
            M_ResponseMessage_KUOCVM_VayHaiKheUocCungMotKyHan, //Vay hai khế ước cùng một sản phẩm

            //Số giao dịch
            M_ResponseMessage_SoGiaoDich_TonTai, //tồn tại
            M_ResponseMessage_SoGiaoDich_KhongTonTai, //Không tồn tại
            M_ResponseMessage_SoGiaoDich_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_SoGiaoDich_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_SoGiaoDich_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_SoGiaoDich_TaoKhongThanhCong, //tạo không thành công

            //Giao dịch
            M_ResponseMessage_GIAODICH_KhongTonTai, //Không tồn tại
            M_ResponseMessage_GIAODICH_KhongThuocKiemSoatCuaPhongGD, //Giao dịch không thuộc kiểm soát của phòng giao dịch
            M_ResponseMessage_GIAODICH_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_GIAODICH_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_GIAODICH_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch
            M_ResponseMessage_GIAODICH_TonTai, //tồn tại
            M_ResponseMessage_GIAODICH_ThongTinDuocPhepSua, //Giao dịch bị sửa những thông tin giao dịch được phép sửa
            M_ResponseMessage_GIAODICH_ThongTinKhongDuocPhepSua, //Giao dịch bị sửa những thông tin không được phép sửa
            M_ResponseMessage_GIAODICH_ThemThanhCong, //Thêm giao dịch thành công
            M_ResponseMessage_GIAODICH_SuaThanhCong, //Sửa giao dịch thành công
            M_ResponseMessage_GIAODICH_XoaThanhCong, //Xóa giao dịch thành công
            M_ResponseMessage_GIAODICH_DuyetThanhCong, //Duyệt giao dịch thành công
            M_ResponseMessage_GIAODICH_ThoaiDuyetThanhCong, //Thoái duyệt giao dịch thành công
            M_ResponseMessage_GIAODICH_TuChoiDuyetThanhCong, //Từ chối duyệt giao dịch thành công
            M_ResponseMessage_GIAODICH_ThemKhongThanhCong, //Thêm giao dịch không thành công
            M_ResponseMessage_GIAODICH_SuaKhongThanhCong, //Sửa giao dịch không thành công
            M_ResponseMessage_GIAODICH_XoaKhongThanhCong, //Xóa giao dịch không thành công
            M_ResponseMessage_GIAODICH_DuyetKhongThanhCong, //Duyệt giao dịch không thành công
            M_ResponseMessage_GIAODICH_ThoaiDuyetKhongThanhCong, //Thoái duyệt giao dịch không thành công
            M_ResponseMessage_GIAODICH_TuChoiDuyetKhongThanhCong, //Từ chối duyệt giao dịch không thành công
            M_ResponseMessage_GIAODICH_TaoGiaoDichKhongThanhCong, //tạo giao dịch không thành công
            M_ResponseMessage_GIAODICH_CapNhatThongTinGiaoDichKhongThanhCong, //cập nhât giao dịch không thành công
            M_ResponseMessage_GIAODICH_KhongDuocPhepSua, //không được phép sửa
            M_ResponseMessage_GIAODICH_KhongDuocPhepXoa, //không được phép xóa
            M_ResponseMessage_GIAODICH_KhongDuocPhepDuyet, //không được phép duyệt
            M_ResponseMessage_GIAODICH_KhongDuocPhepThoaiDuyet, //không được phép thoái duyệt
            M_ResponseMessage_GIAODICH_KhongDuocPhepTuChoiDuyet, //không được phép từ chối duyệt
            M_ResponseMessage_GIAODICH_TonTaiGiaoDichChuaDuocDuyet, //tồn tại giao dịch chưa được duyệt


            //Nghiệp vụ kế toán
            M_ResponseMessage_KeToan_ABC,
            M_ResponseMessage_DinhKhoan_KhongDuocGhiNoVaCo,
            M_ResponseMessage_DinhKhoan_ChuaCoGhiNoHoacCo,
            M_ResponseMessage_NhomDinhKhoan_KhongCoGhiNoHoacCo,
            M_ResponseMessage_NhomDinhKhoan_ChiCoGhiNo,
            M_ResponseMessage_NhomDinhKhoan_ChiCoGhiCo,
            M_ResponseMessage_NhomDinhKhoan_NhieuNoNhieuCo,
            M_ResponseMessage_NhomDinhKhoan_TongNoKhongBangTongCo,
            M_ResponseMessage_PhieuKeToan_KhongCoTaiKhoanTienMat,
            M_ResponseMessage_PhieuKeToan_KhongDuocGhiCoTienMat,
            M_ResponseMessage_PhieuKeToan_KhongDuocGhiNoTienMat,
            M_ResponseMessage_PhieuKeToan_KhongDuocTonTaiTienMat,

            M_ResponseMessage_PhanLoaiTaiKhoan_ThemKhongThanhCong,
            M_ResponseMessage_PhanLoaiTaiKhoan_ThemThanhCong,

            M_ResponseMessage_PhanLoaiTaiKhoan_SuaKhongThanhCong,
            M_ResponseMessage_PhanLoaiTaiKhoan_SuaThanhCong,

            M_ResponseMessage_PhanLoaiTaiKhoan_XoaKhongThanhCong,
            M_ResponseMessage_PhanLoaiTaiKhoan_XoaThanhCong,

            M_ResponseMessage_PhanLoaiTaiKhoan_DuyetKhongThanhCong,
            M_ResponseMessage_PhanLoaiTaiKhoan_DuyetThanhCong,

            M_ResponseMessage_PhanLoaiTaiKhoan_ThoaiDuyetKhongThanhCong,
            M_ResponseMessage_PhanLoaiTaiKhoan_ThoaiDuyetThanhCong,

            M_ResponseMessage_PhanLoaiTaiKhoan_TuChoiDuyetKhongThanhCong,
            M_ResponseMessage_PhanLoaiTaiKhoan_TuChoiDuyetThanhCong,

            M_ResponseMessage_KyHan_KhongHopLe,
            M_ResponseMessage_KyHan_HopLe,

            M_ResponseMessage_NgayDaoHan_KhongHopLe,
            M_ResponseMessage_NgayDaoHan_HopLe,

            M_ResponseMessage_KyHanDaoHan_KhongHopLe,
            M_ResponseMessage_KyHanDaoHan_HopLe,

            //Nhân sự
            M_ResponseMessage_HoSoNhanSu_KhongTonTai, //Hồ sơ không tồn tại            
            M_ResponseMessage_HoSoNhanSu_KhongThuocPhongGD, //Hồ sơ không kiểm soát của phòng
            M_ResponseMessage_HoSoNhanSu_KhongTaoDuoc, //Không tạo được mã hồ sơ
            M_ResponseMessage_HopDong_KhongTaoDuocMa, //Không tạo được số hợp đồng lao động
            M_ResponseMessage_HopDong_KhongTonTai, //Hợp đồng không tồn tại            
            M_ResponseMessage_HopDong_KhongThuocPhongGD, //Hợp đồng không kiểm soát của phòng
            M_ResponseMessage_ThuyenChuyenCongTac_KhongTaoDuocMa, //Không tạo được số mã thuyên chuyển công tác
            M_ResponseMessage_ThuyenChuyenCongTac_KhongTonTai, //Thuyên chuyển công tác không tồn tại            
            M_ResponseMessage_ThuyenChuyenCongTac_KhongThuocPhongGD, //Thuyên chuyển công tác không kiểm soát của phòng
            M_ResponseMessage_ThoiViec_KhongTaoDuocMa, //Không tạo được số mã thôi việc
            M_ResponseMessage_ThoiViec_KhongTonTai, //Thôi việc không tồn tại            
            M_ResponseMessage_ThoiViec_KhongThuocPhongGD,//Thôi việc không kiểm soát của phòng
            M_ResponseMessage_ThongTinLuong_KhongTonTai, //Thông tin lương không tồn tại            
            M_ResponseMessage_ThongTinLuong_KhongThuocPhongGD,//Thông tin lương không kiểm soát của phòng

            //Ty gia
            //Tiền tệ
            M_ResponseMessage_TyGia_KhongTonTai, //Không tồn tại 
            M_ResponseMessage_TyGia_KhongDuocPhepHoatDong, //tồn tại nhưng không được phép hoạt động
            M_ResponseMessage_TyGia_ChuaDuocPhepGiaoDich, //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
            M_ResponseMessage_TyGia_DuocPhepGiaoDich, //tồn tại, đang hoạt động, và được phép giao dịch

            //Giao dich doi tuong
            M_ResponseMessage_DoiTuongGDich_KhongTaoDuocDoiTuong,
            M_ResponseMessage_DoiTuongGDich_KhongTaoDuocMaDoiTuong,
            M_ResponseMessage_DoiTuongGDich_ThemDoiTuongKhongThanhCong,
            M_ResponseMessage_DoiTuongGDich_DaTonTaiMaGoiNho,

            //Giao dich doi tuong
            M_ResponseMessage_DoiTuong_TinhChatSoDuKhongHopLe,
            M_ResponseMessage_DoiTuong_KhongTaoDuocMaDoiTuong,
            M_ResponseMessage_DoiTuong_ThemDoiTuongKhongThanhCong,
            M_ResponseMessage_DoiTuong_TongSoDuKhongDung,

            //Han muc
            M_ResponseMessage_HanMuc_HanMucPheDuyetLonHonHMPheDuyetCapTong,
            M_ResponseMessage_HanMuc_ChuaNhapTaiSanDamBao,
            M_ResponseMessage_HanMuc_TongTSDBToiDaLonHonHMCoTSDB,

            //Vong vay von
            M_ResponseMessage_VongVay_KhongHopLe,
            M_ResponseMessage_VongVay_KhongTonTai,
            M_ResponseMessage_VongVay_KhongDuocPhepHoatDong,
            M_ResponseMessage_VongVay_ChuaDuocPhepGiaoDich,

            //Don xin vay
            M_ResponseMessage_DonVayVon_KhongHopLe,
            M_ResponseMessage_DonVayVon_KhongTonTai,
            M_ResponseMessage_DonVayVon_KhongDuocPhepHoatDong,
            M_ResponseMessage_DonVayVon_ChuaDuocPhepGiaoDich,
            M_ResponseMessage_DonVayVon_VuotHanMucGocVay,
            M_ResponseMessage_DonVayVon_VuotHanMucThoiHanVay,
            M_ResponseMessage_Nhom_SoLuongKhachHangNhoHonQuyDinh,
            M_ResponseMessage_Nhom_SoLuongKhachHangLonHonQuyDinh,

            //Khe uoc
            M_ResponseMessage_KheUoc_ThemThanhCong,
            M_ResponseMessage_KheUoc_ThemKhongThanhCong,
            M_ResponseMessage_KheUoc_DuyetThanhCong,
            M_ResponseMessage_KheUoc_DuyetKhongThanhCong,
            M_ResponseMessage_KheUoc_TonTaiKiemSoatRuiRo,

            //Kiem soat rui ro
            M_ResponseMessage_KiemSoatRuiRo_KheUocDaGiaiNgan,
            M_ResponseMessage_KiemSoatRuiRo_ThemMaXacNhan_KhongThanhCong,
            M_ResponseMessage_KiemSoatRuiRo_ThemMaXacNhan_ThanhCong,
            M_ResponseMessage_KiemSoatRuiRo_CapNhatMaXacNhan_ThanhCong,
            M_ResponseMessage_KiemSoatRuiRo_CapNhatMaXacNhan_KhongThanhCong,
            M_ResponseMessage_KiemSoatRuiRo_XoaMaXacNhan_ThanhCong,
            M_ResponseMessage_KiemSoatRuiRo_XoaMaXacNhan_KhongThanhCong,
            M_ResponseMessage_KiemSoatRuiRo_TaoMaXacNhan_ThanhCong,
            M_ResponseMessage_KiemSoatRuiRo_TaoMaXacNhan_KhongThanhCong,

            M_ResponseMessage_Email_QueueKhongTonTai,
            M_ResponseMessage_Email_QueueThanhCong,
            M_ResponseMessage_Email_QueueHoanThanh,

            //Lãi suất trần sàn
            M_ResponseMessage_LaiSuatSanKKH_KhongHopLe,
            M_ResponseMessage_LaiSuatTranKKH_KhongHopLe,
            M_ResponseMessage_LaiSuatSanCKH_KhongHopLe,
            M_ResponseMessage_LaiSuatTranCKH_KhongHopLe
        }; 

        public static string layGiaTri(this NghiepVuResponseMessage nghiepVuResponseMessage)
        {
            switch (nghiepVuResponseMessage)
            {
                //Hệ thống
                case NghiepVuResponseMessage.M_ResponseMessage_HeThong_DangGiaoDich: return "M_ResponseMessage_HeThong_DangGiaoDich";//Hệ thống đang giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_HeThong_TamNgungGiaoDich: return "M_ResponseMessage_HeThong_TamNgungGiaoDich";//Hệ thống tạm ngừng giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_HeThong_NgungGiaoDich: return "M_ResponseMessage_HeThong_NgungGiaoDich";//Hệ thống ngừng giao dịch

                //Dùng chung
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_ThanhCong: return "M_ResponseMessage_DungChung_ThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_KhongThanhCong: return "M_ResponseMessage_DungChung_KhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_LoiKhongXacDinh: return "M_ResponseMessage_DungChung_LoiKhongXacDinh";
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_KhongTonTaiDichVuNghiepVu: return "M_ResponseMessage_DungChung_KhongTonTaiDichVuNghiepVu";
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_KhongTonTaiChucNangXuLy: return "M_ResponseMessage_DungChung_KhongTonTaiChucNangXuLy";
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_DaDuocSuDung: return "M_ResponseMessage_DungChung_DaDuocSuDung";//Đã được sử dụng không thể xóa
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_KhongTonTai: return "M_ResponseMessage_DungChung_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_KhongThuocDonVi: return "M_ResponseMessage_DungChung_KhongThuocDonVi";//Không thuộc đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_DungChung_KhongThuocPhongGiaoDich: return "M_ResponseMessage_DungChung_KhongThuocPhongGiaoDich";//Không thuộc phòng giao dịch

                //Hành động
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_KhongDuDieuKienDeLuuTam: return "M_ResponseMessage_HanhDong_KhongDuDieuKienDeLuuTam";//Không đủ điều kiện để lưu tạm
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_ThemThanhCong: return "M_ResponseMessage_HanhDong_ThemThanhCong"; //Thêm thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_ThemKhongThanhCong: return "M_ResponseMessage_HanhDong_ThemKhongThanhCong"; //Thêm không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_SuaThanhCong: return "M_ResponseMessage_HanhDong_SuaThanhCong"; //Sửa thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_SuaKhongThanhCong: return "M_ResponseMessage_HanhDong_SuaKhongThanhCong"; //Sửa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_XoaThanhCong: return "M_ResponseMessage_HanhDong_XoaThanhCong"; //Xóa thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_XoaKhongThanhCong: return "M_ResponseMessage_HanhDong_XoaKhongThanhCong"; //Xóa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_DuyetThanhCong: return "M_ResponseMessage_HanhDong_DuyetThanhCong"; //Duyệt thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_DuyetKhongThanhCong: return "M_ResponseMessage_HanhDong_DuyetKhongThanhCong"; //Duyệt không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_ThoaiDuyetThanhCong: return "M_ResponseMessage_HanhDong_ThoaiDuyetThanhCong"; //Thoái duyệt thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_ThoaiDuyetKhongThanhCong: return "M_ResponseMessage_HanhDong_ThoaiDuyetKhongThanhCong"; //Thoái duyệt không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_TuChoiDuyetThanhCong: return "M_ResponseMessage_HanhDong_TuChoiDuyetThanhCong"; //Từ chối duyệt thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_TuChoiDuyetKhongThanhCong: return "M_ResponseMessage_HanhDong_TuChoiDuyetKhongThanhCong"; //Từ chối duyệt không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_XuLyThanhCong: return "M_ResponseMessage_HanhDong_XuLyThanhCong"; //Xử lý thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_XuLyKhongThanhCong: return "M_ResponseMessage_HanhDong_XuLyKhongThanhCong"; //Xử lý không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_LuuTamThanhCong: return "M_ResponseMessage_HanhDong_LuuTamThanhCong"; //Lưu tạm thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_LuuTamKhongThanhCong: return "M_ResponseMessage_HanhDong_LuuTamKhongThanhCong"; //Lưu tạm không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_TrinhDuyetThanhCong: return "M_ResponseMessage_HanhDong_TrinhDuyetThanhCong"; //Trình duyệt thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_TrinhDuyetKhongThanhCong: return "M_ResponseMessage_HanhDong_TrinhDuyetKhongThanhCong"; //Trình duyệt không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong: return "M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong"; //Lấy dữ liệu không thành công
                    
                //Mã
                case NghiepVuResponseMessage.M_ResponseMessage_Ma_KhongTonTai: return "M_ResponseMessage_Ma_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_Ma_DaTonTai: return "M_ResponseMessage_Ma_DaTonTai";//Đã tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_Ma_KhongTaoDuoc: return "M_ResponseMessage_Ma_KhongTaoDuoc";//Không tạo được
                    
                //Dữ liệu
                case NghiepVuResponseMessage.M_ResponseMessage_DuLieu_KhongDuocPhepThayDoi: return "M_ResponseMessage_DuLieu_KhongDuocPhepThayDoi";//Dữ liệu không được phép thay đổi
                case NghiepVuResponseMessage.M_ResponseMessage_DuLieu_DaDuocSuDungKhongDuocXoa: return "M_ResponseMessage_DuLieu_DaDuocSuDungKhongDuocXoa";//Dữ liệu đã được sử dụng không được xóa
                case NghiepVuResponseMessage.M_ResponseMessage_DuLieu_DaDuocSuDungKhongDuocSua: return "M_ResponseMessage_DuLieu_DaDuocSuDungKhongDuocSua";//Dữ liệu đã được sử dụng không được sửa

                //Hình thức giao dịch 
                case NghiepVuResponseMessage.M_ResponseMessage_HinhThucGiaoDich_KhongTonTai: return "M_ResponseMessage_HinhThucGiaoDich_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_HinhThucGiaoDich_KhongDuocPhepHoatDong: return "M_ResponseMessage_HinhThucGiaoDich_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_HinhThucGiaoDich_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_HinhThucGiaoDich_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_HinhThucGiaoDich_DuocPhepGiaoDich: return "M_ResponseMessage_HinhThucGiaoDich_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Chỉ thị đáo hạn
                case NghiepVuResponseMessage.M_ResponseMessage_ChiThiDaoHan_KhongTonTai: return "M_ResponseMessage_ChiThiDaoHan_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_ChiThiDaoHan_KhongDuocPhepHoatDong: return "M_ResponseMessage_ChiThiDaoHan_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_ChiThiDaoHan_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_ChiThiDaoHan_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_ChiThiDaoHan_DuocPhepGiaoDich: return "M_ResponseMessage_ChiThiDaoHan_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Loại giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGiaoDich_KhongTonTai: return "M_ResponseMessage_LoaiGiaoDich_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGiaoDich_KhongDuocPhepHoatDong: return "M_ResponseMessage_LoaiGiaoDich_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGiaoDich_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_LoaiGiaoDich_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGiaoDich_DuocPhepGiaoDich: return "M_ResponseMessage_LoaiGiaoDich_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Mã đối tượng
                case NghiepVuResponseMessage.M_ResponseMessage_MaDoiTuong_KhongTonTai: return "M_ResponseMessage_MaDoiTuong_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaDoiTuong_KhongDuocPhepHoatDong: return "M_ResponseMessage_MaDoiTuong_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_MaDoiTuong_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_MaDoiTuong_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_MaDoiTuong_DuocPhepGiaoDich: return "M_ResponseMessage_MaDoiTuong_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Mã đối tượng
                case NghiepVuResponseMessage.M_ResponseMessage_MucDichVay_KhongTonTai: return "M_ResponseMessage_MucDichVay_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MucDichVay_KhongDuocPhepHoatDong: return "M_ResponseMessage_MucDichVay_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_MucDichVay_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_MucDichVay_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_MucDichVay_DuocPhepGiaoDich: return "M_ResponseMessage_MucDichVay_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Loại chứng từ
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiChungTu_KhongTonTai: return "M_ResponseMessage_LoaiChungTu_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiChungTu_KhongDuocPhepHoatDong: return "M_ResponseMessage_LoaiChungTu_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiChungTu_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_LoaiChungTu_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiChungTu_DuocPhepGiaoDich: return "M_ResponseMessage_LoaiChungTu_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Trạng thái nghiệp vụ
                case NghiepVuResponseMessage.M_ResponseMessage_TrangThaiNghiepVu_KhongTonTai: return "M_ResponseMessage_TrangThaiNghiepVu_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_TrangThaiNghiepVu_KhongDuocPhepHoatDong: return "M_ResponseMessage_TrangThaiNghiepVu_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TrangThaiNghiepVu_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TrangThaiNghiepVu_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TrangThaiNghiepVu_DuocPhepGiaoDich: return "M_ResponseMessage_TrangThaiNghiepVu_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TrangThaiNghiepVu_KhongHopLe: return "M_ResponseMessage_TrangThaiNghiepVu_KhongHopLe";//Ko hợp lệ

                //Khu vực
                case NghiepVuResponseMessage.M_ResponseMessage_KhuVuc_KhongTonTai: return "M_ResponseMessage_KhuVuc_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_KhuVuc_KhongDuocPhepHoatDong: return "M_ResponseMessage_KhuVuc_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_KhuVuc_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_KhuVuc_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_KhuVuc_DuocPhepGiaoDich: return "M_ResponseMessage_KhuVuc_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                //Cụm
                case NghiepVuResponseMessage.M_ResponseMessage_Cum_KhongTonTai: return "M_ResponseMessage_Cum_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_Cum_KhongDuocPhepHoatDong: return "M_ResponseMessage_Cum_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_Cum_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_Cum_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_Cum_DuocPhepGiaoDich: return "M_ResponseMessage_Cum_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                //Nhóm
                case NghiepVuResponseMessage.M_ResponseMessage_Nhom_KhongTonTai: return "M_ResponseMessage_Nhom_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_Nhom_KhongDuocPhepHoatDong: return "M_ResponseMessage_Nhom_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_Nhom_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_Nhom_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_Nhom_DuocPhepGiaoDich: return "M_ResponseMessage_Nhom_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                //Địa bàn
                case NghiepVuResponseMessage.M_ResponseMessage_DiaBan_KhongTonTai: return "M_ResponseMessage_DiaBan_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_DiaBan_KhongDuocPhepHoatDong: return "M_ResponseMessage_DiaBan_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_DiaBan_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_DiaBan_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_DiaBan_DuocPhepGiaoDich: return "M_ResponseMessage_DiaBan_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                //Tỉnh thành phố
                case NghiepVuResponseMessage.M_ResponseMessage_TinhTP_KhongTonTai: return "M_ResponseMessage_TinhTP_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_TinhTP_KhongDuocPhepHoatDong: return "M_ResponseMessage_TinhTP_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TinhTP_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TinhTP_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TinhTP_DuocPhepGiaoDich: return "M_ResponseMessage_TinhTP_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                //Tiền tệ
                case NghiepVuResponseMessage.M_ResponseMessage_TienTe_KhongTonTai: return "M_ResponseMessage_TienTe_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_TienTe_KhongDuocPhepHoatDong: return "M_ResponseMessage_TienTe_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TienTe_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TienTe_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TienTe_DuocPhepGiaoDich: return "M_ResponseMessage_TienTe_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                //Biểu phí
                case NghiepVuResponseMessage.M_ResponseMessage_BieuPhi_KhongTonTai: return "M_ResponseMessage_BieuPhi_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_BieuPhi_KhongDuocPhepHoatDong: return "M_ResponseMessage_BieuPhi_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_BieuPhi_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_BieuPhi_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_BieuPhi_DuocPhepGiaoDich: return "M_ResponseMessage_BieuPhi_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_BieuPhi_HetHieuLuc: return "M_ResponseMessage_BieuPhi_HetHieuLuc"; //hết hiệu lực
                case NghiepVuResponseMessage.M_ResponseMessage_BieuPhi_ConHieuLuc: return "M_ResponseMessage_BieuPhi_ConHieuLuc"; //Còn hiệu lực

                //Đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_DonVi_KhongTonTai: return "M_ResponseMessage_DonVi_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_DonVi_KhongDuocPhepHoatDong: return "M_ResponseMessage_DonVi_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_DonVi_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_DonVi_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_DonVi_DuocPhepGiaoDich: return "M_ResponseMessage_DonVi_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongTonTai: return "M_ResponseMessage_KhachHang_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongDuocPhepHoatDong: return "M_ResponseMessage_KhachHang_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_KhachHang_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_DuocPhepGiaoDich: return "M_ResponseMessage_KhachHang_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongThuocDonVi: return "M_ResponseMessage_KhachHang_KhongThuocDonVi";//khách hàng không thuộc đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ThuocDonVi: return "M_ResponseMessage_KhachHang_ThuocDonVi";//khách hàng thuộc đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_LaThanhVien: return "M_ResponseMessage_KhachHang_LaThanhVien";//khách hàng là thành viên
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongLaThanhVien: return "M_ResponseMessage_KhachHang_KhongLaThanhVien";//khách hàng không là thành viên
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_CoTuoiKhongHopLe: return "M_ResponseMessage_KhachHang_CoTuoiKhongHopLe";//khách hàng có tuổi không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_CoTuoiHopLe: return "M_ResponseMessage_KhachHang_CoTuoiHopLe";//khách hàng có tuổi hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_HetHieuLuc: return "M_ResponseMessage_KhachHang_HetHieuLuc";//khách hàng hết hiệu lực
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ConHieuLuc: return "M_ResponseMessage_KhachHang_ConHieuLuc";//khách hàng còn hiệu lực
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_HoKhauKhongThuocDiaPhuong: return "M_ResponseMessage_KhachHang_HoKhauKhongThuocDiaPhuong";//Hộ khẩu thường trú của khách hàng không thuộc địa phương
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_HoKhauThuocDiaPhuong: return "M_ResponseMessage_KhachHang_HoKhauThuocDiaPhuong";//Hộ khẩu thường trú của khách hàng thuộc địa phương
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoKyNoTKQDChuaDuTieuChuan: return "M_ResponseMessage_KhachHang_SoKyNoTKQDChuaDuTieuChuan";//Số kỳ nộp tiết kiệm quy định của khách hàng chưa đủ tiêu chuẩn
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoKyNoTKQDDuTieuChuan: return "M_ResponseMessage_KhachHang_SoKyNoTKQDDuTieuChuan";//Số kỳ nộp tiết kiệm quy định của khách hàng đủ tiêu chuẩn
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoKyNoTKQDDuKhongLayDuoc: return "M_ResponseMessage_KhachHang_SoKyNoTKQDDuKhongLayDuoc";//không lấy được Số kỳ nộp tiết kiệm quy định của khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongDangCoHDTD: return "M_ResponseMessage_KhachHang_KhongDangCoHDTD";//Khách hàng không đang có hợp đồng tín dụng
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_DangCoHDTD: return "M_ResponseMessage_KhachHang_DangCoHDTD";//Khách hàng đang có hợp đồng tín dụng
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongDangDongTrachNhiem: return "M_ResponseMessage_KhachHang_KhongDangDongTrachNhiem";//Khách hàng không đang đồng trách nhiệm với hợp đồng khác
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_DangDongTrachNhiem: return "M_ResponseMessage_KhachHang_DangDongTrachNhiem";//Khách hàng đang đồng trách nhiệm với hợp đồng khác
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongDuocPhepTrong: return "M_ResponseMessage_KhachHang_KhongDuocPhepTrong";//Mã khách hàng không được phép null hoặc trống
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_LaChuTaiKhoan: return "M_ResponseMessage_KhachHang_LaChuTaiKhoan";//khách hàng là chủ tài khoản
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongLaChuTaiKhoan: return "M_ResponseMessage_KhachHang_KhongLaChuTaiKhoan";//khách hàng không là chủ tài khoản
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ThuocDanhSachDCSH: return "M_ResponseMessage_KhachHang_ThuocDanhSachDCSH";//khách hàng thuộc danh sách đồng chủ sở hữu
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhongHopLe: return "M_ResponseMessage_KhachHang_KhongHopLe"; // khách hàng không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ConDuNo: return "M_ResponseMessage_KhachHang_ConDuNo"; // khách hàng còn dư nợ
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ConSoDuTietKiem: return "M_ResponseMessage_KhachHang_ConSoDuTietKiem";  // khách hàng còn số dư tiết kiệm
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ThemDuLieuAnhKhongThanhCong: return "M_ResponseMessage_KhachHang_ThemDuLieuAnhKhongThanhCong"; // thêm dữ liệu ảnh không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoCMNDDaTonTai: return "M_ResponseMessage_KhachHang_SoCMNDDaTonTai"; // Số chứng minh nhân dân đã tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoHoKhauDaTonTai: return "M_ResponseMessage_KhachHang_SoHoKhauDaTonTai"; // Số chứng minh nhân dân đã tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoLuongKHDaDuTrongNhom: return "M_ResponseMessage_KhachHang_SoLuongKHDaDuTrongNhom";  // thêm dữ liệu ảnh không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KoTaoDuocMaKhachHang: return "M_ResponseMessage_KhachHang_KoTaoDuocMaKhachHang";  // Không tạo được mã khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_KhachHangKoThuocPGD: return "M_ResponseMessage_KhachHang_KhachHangKoThuocPGD";  // Khách hàng ko thuộc kiểm soát của phòng giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_DuyetThanhCong: return "M_ResponseMessage_KhachHang_DuyetThanhCong";  // Duyệt khách hàng thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_DuyetKhongThanhCong: return "M_ResponseMessage_KhachHang_DuyetKhongThanhCong";  // Duyệt khách hàng không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoCMNDKhongHopLe: return "M_ResponseMessage_KhachHang_SoCMNDKhongHopLe"; // Số chứng minh nhân dân không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_ConKhoanVayCungHoKhau: return "M_ResponseMessage_KhachHang_ConKhoanVayCungHoKhau"; // Con khoan vay cung ho khau
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoDienThoaiDaTonTai: return "M_ResponseMessage_KhachHang_SoDienThoaiDaTonTai"; // Số điện thoại đã tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoDiDongDaTonTai: return "M_ResponseMessage_KhachHang_SoDiDongDaTonTai"; // Số di động đã tồn tại

                //Nhóm nợ
                case NghiepVuResponseMessage.M_ResponseMessage_NhomNo_HopLe: return "M_ResponseMessage_NhomNo_HopLe"; //Hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NhomNo_KhongHopLe: return "M_ResponseMessage_NhomNo_KhongHopLe"; //Không Hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NhomNo_LonHonNhomNoHienTai: return "M_ResponseMessage_NhomNo_LonHonNhomNoHienTai"; //Nhóm nợ mới lớn hơn nhóm nợ hiện  tại
                case NghiepVuResponseMessage.M_ResponseMessage_NhomNo_BangNhomNoHienTai: return "M_ResponseMessage_NhomNo_BangNhomNoHienTai"; //Nhóm nợ mới bằng nhóm nợ hiện  tại
                case NghiepVuResponseMessage.M_ResponseMessage_NhomNo_NhoHonNhomNoHienTai: return "M_ResponseMessage_NhomNo_NhoHonNhomNoHienTai"; //Nhóm nợ mới nhỏ hơn nhóm nợ hiện  tại

                //Đồng chủ sở hữu
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_KhongTonTai: return "M_ResponseMessage_DongChuSoHuu_KhongTonTai";//Không tồn tại nhân sự
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_KhongDuocPhepHoatDong: return "M_ResponseMessage_DongChuSoHuu_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_DongChuSoHuu_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_DuocPhepGiaoDich: return "M_ResponseMessage_DongChuSoHuu_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_XoaKhongThanhCong: return "M_ResponseMessage_DongChuSoHuu_XoaKhongThanhCong";//Xóa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_ThemKhongThanhCong: return "M_ResponseMessage_DongChuSoHuu_ThemKhongThanhCong";//Thêm không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_SuaKhongThanhCong: return "M_ResponseMessage_DongChuSoHuu_SuaKhongThanhCong"; //Sửa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_DongChuSoHuu_SuaThanhCong: return "M_ResponseMessage_DongChuSoHuu_SuaThanhCong"; //Sửa thành công

                //Nhân sự
                case NghiepVuResponseMessage.M_ResponseMessage_NhanSu_KhongTonTai: return "M_ResponseMessage_NhanSu_KhongTonTai";//Không tồn tại nhân sự
                case NghiepVuResponseMessage.M_ResponseMessage_NhanSu_KhongDuocPhepHoatDong: return "M_ResponseMessage_NhanSu_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_NhanSu_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_NhanSu_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NhanSu_DuocPhepGiaoDich: return "M_ResponseMessage_NhanSu_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NhanSu_KhongThuocDonViTacNghiep: return "M_ResponseMessage_NhanSu_KhongThuocDonViTacNghiep";/// không thuộc đơn vị tác nghiệp
                case NghiepVuResponseMessage.M_ResponseMessage_NhanSu_ThuocDonViTacNghiep: return "M_ResponseMessage_NhanSu_ThuocDonViTacNghiep";/// thuộc đơn vị tác nghiệp

                //Nhóm Sản phẩm huy động vốn
                case NghiepVuResponseMessage.M_ResponseMessage_NhomSanPhamHuyDongVon_KhongHopLe: return "M_ResponseMessage_NhomSanPhamHuyDongVon_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NhomSanPhamHuyDongVon_HopLe: return "M_ResponseMessage_NhomSanPhamHuyDongVon_HopLe";//Hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NhomSanPhamHuyDongVon_KhongTonTai: return "M_ResponseMessage_NhomSanPhamHuyDongVon_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_NhomSanPhamHuyDongVon_KhongDuocPhepHoatDong: return "M_ResponseMessage_NhomSanPhamHuyDongVon_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_NhomSanPhamHuyDongVon_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_NhomSanPhamHuyDongVon_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NhomSanPhamHuyDongVon_DuocPhepGiaoDich: return "M_ResponseMessage_NhomSanPhamHuyDongVon_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch,

                //Sản phẩm huy động vốn
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongTonTai: return "M_ResponseMessage_SanPhamHuyDongVon_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepHoatDong: return "M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_SanPhamHuyDongVon_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_DuocPhepGiaoDich: return "M_ResponseMessage_SanPhamHuyDongVon_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch,
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_HetHieuLuc: return "M_ResponseMessage_SanPhamHuyDongVon_HetHieuLuc";//hết hiệu lực
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepLapLichTraLai: return "M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepLapLichTraLai";//Không được phép lập lịch trả lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepLapLichGuiGop: return "M_ResponseMessage_SanPhamHuyDongVon_KhongDuocPhepLapLichGuiGop";//Không được phép lập lịch gửi góp
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemQuyDinh: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemQuyDinh";//Không thuộc nhóm sp Tiết kiệm quy định
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemKhongKyHan: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemKhongKyHan";//Không thuộc nhóm sp Tiết kiệm tự nguyện không kỳ hạn
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiDinhKy: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiDinhKy";//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn trả lãi định kỳ
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiSau: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiSau";//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn trả lãi sau
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiTruoc: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanTraLaiTruoc";//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn trả lãi trước
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanGuiGop: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHanGuiGop";//Không thuộc nhóm sp Tiết kiệm tự nguyện có kỳ hạn gửi góp
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHan: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTietKiemCoKyHan";//Không thuộc nhóm sp Tiền gửi có kỳ hạn
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTienGuiThanhToan: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocNhomSPTienGuiThanhToan";//Không thuộc nhóm sp Tiền gửi thanh toán
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongDuocLapLichTraLai: return "M_ResponseMessage_SanPhamHuyDongVon_KhongDuocLapLichTraLai"; //Không được lập lịch trả lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_DuocLapLichTraLai: return "M_ResponseMessage_SanPhamHuyDongVon_DuocLapLichTraLai"; //Được lập lịch trả lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongDuocLapLichTraGop: return "M_ResponseMessage_SanPhamHuyDongVon_KhongDuocLapLichTraGop"; //Không được lập lịch trả góp
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_DuocLapLichTraGop: return "M_ResponseMessage_SanPhamHuyDongVon_DuocLapLichTraGop"; //Được lập lịch trả góp
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongTatToanTruocHan: return "M_ResponseMessage_SanPhamHuyDongVon_KhongTatToanTruocHan"; //Không được tất toán trước hạn
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_CoTatToanTruocHan: return "M_ResponseMessage_SanPhamHuyDongVon_CoTatToanTruocHan"; //Được tất toán trước hạn
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamHuyDongVon_KhongThuocKiemSoatCuaDonVi: return "M_ResponseMessage_SanPhamHuyDongVon_KhongThuocKiemSoatCuaDonVi"; //Sản phẩm không thuộc kiểm soát của đơn vị                

                //Lãi suất
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_KhongTonTai: return "M_ResponseMessage_MaLaiSuat_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_KhongDuocPhepHoatDong: return "M_ResponseMessage_MaLaiSuat_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_MaLaiSuat_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_DuocPhepGiaoDich: return "M_ResponseMessage_MaLaiSuat_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_HetHieuLuc: return "M_ResponseMessage_MaLaiSuat_HetHieuLuc";//hết hiệu lực
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_KhongHopLe: return "M_ResponseMessage_MaLaiSuat_KhongHopLe";//Mã lãi suất không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_HopLe: return "M_ResponseMessage_MaLaiSuat_HopLe";//Mã lãi suất hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_KhongThuocPhanHeNghiepVu: return "M_ResponseMessage_MaLaiSuat_KhongThuocPhanHeNghiepVu";//Mã lãi suất không thuộc phân hệ nghiệp vụ
                case NghiepVuResponseMessage.M_ResponseMessage_MaLaiSuat_ThuocPhanHeNghiepVu: return "M_ResponseMessage_MaLaiSuat_ThuocPhanHeNghiepVu";//Mã lãi suất thuộc phân hệ nghiệp vụ
                case NghiepVuResponseMessage.M_ResponseMessage_DVTinhLaiSuat_KhongTonTai: return "M_ResponseMessage_DVTinhLaiSuat_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_DVTinhLaiSuat_KhongDuocPhepHoatDong: return "M_ResponseMessage_DVTinhLaiSuat_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_DVTinhLaiSuat_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_DVTinhLaiSuat_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_DVTinhLaiSuat_DuocPhepGiaoDich: return "M_ResponseMessage_DVTinhLaiSuat_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Tỷ lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TyLe_KhongHopLe: return "M_ResponseMessage_TyLe_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TyLe_HopLe: return "M_ResponseMessage_TyLe_HopLe";//hợp lệ

                //Hệ số
                case NghiepVuResponseMessage.M_ResponseMessage_HeSo_KhongHopLe: return "M_ResponseMessage_HeSo_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_HeSo_HopLe: return "M_ResponseMessage_HeSo_HopLe";//hợp lệ

                //Tần suất
                case NghiepVuResponseMessage.M_ResponseMessage_TanSuat_KhongHopLe: return "M_ResponseMessage_TanSuat_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TanSuat_HopLe: return "M_ResponseMessage_TanSuat_HopLe";//hợp lệ

                //Số kỳ hạn
                case NghiepVuResponseMessage.M_ResponseMessage_SoKyHan_KhongHopLe: return "M_ResponseMessage_SoKyHan_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoKyHan_HopLe: return "M_ResponseMessage_SoKyHan_HopLe";//hợp lệ

                //Số tháng
                case NghiepVuResponseMessage.M_ResponseMessage_SoThang_KhongHopLe: return "M_ResponseMessage_SoThang_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoThang_HopLe: return "M_ResponseMessage_SoThang_HopLe";//hợp lệ

                //Số ngày
                case NghiepVuResponseMessage.M_ResponseMessage_SoNgay_KhongHopLe: return "M_ResponseMessage_SoNgay_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoNgay_HopLe: return "M_ResponseMessage_SoNgay_HopLe";//hợp lệ

                //Tiền lãi
                case NghiepVuResponseMessage.M_ResponseMessage_TienLai_TinhThanhCong: return "M_ResponseMessage_TienLai_TinhThanhCong"; //tính không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_TienLai_TinhKhongThanhCong: return "M_ResponseMessage_TienLai_TinhKhongThanhCong"; //Tính thành công

                //Số tiền lãi tháng
                case NghiepVuResponseMessage.M_ResponseMessage_TienLaiThang_KhongHopLe: return "M_ResponseMessage_TienLaiThang_KhongHopLe"; //không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TienLaiThang_HopLe: return "M_ResponseMessage_TienLaiThang_HopLe"; //hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TienLaiNgay_KhongHopLe: return "M_ResponseMessage_TienLaiNgay_KhongHopLe"; //không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TienLaiNgay_HopLe: return "M_ResponseMessage_TienLaiNgay_HopLe"; //không hợp lệ

                //Số tiền phong tỏa
                case NghiepVuResponseMessage.M_ResponseMessage_SoTienPhongToa_KhongHopLe: return "M_ResponseMessage_SoTienPhongToa_KhongHopLe"; //không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTienPhongToa_HopLe: return "M_ResponseMessage_SoTienPhongToa_HopLe"; //hợp lệ

                //Số tiền kỳ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTienKy_KhongHopLe: return "M_ResponseMessage_SoTienKy_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTienKy_HopLe: return "M_ResponseMessage_SoTienKy_HopLe";//hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTienKy_KhongLayDuoc: return "M_ResponseMessage_SoTienKy_KhongLayDuoc";//Không lấy được

                //Số tiền
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_KhongHopLe: return "M_ResponseMessage_SoTien_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_HopLe: return "M_ResponseMessage_SoTien_HopLe";//hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_BangTongLaiThangVaLaiNgay: return "M_ResponseMessage_SoTien_BangTongLaiThangVaLaiNgay";//Số tiền rút lãi Bằng tổng lãi tháng và lãi ngày
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_KhongBangTongLaiThangVaLaiNgay: return "M_ResponseMessage_SoTien_KhongBangTongLaiThangVaLaiNgay";//Số tiền rút lãi không Bằng tổng lãi tháng và lãi ngày
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_BangTongRutGocVaRutLai: return "M_ResponseMessage_SoTien_BangTongRutGocVaRutLai";//Số tiền giao dịch bằng tổng số tiền rút gốc và số tiền rút lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_KhongBangTongRutGocVaRutLai: return "M_ResponseMessage_SoTien_KhongBangTongRutGocVaRutLai";//Số tiền giao dịch không bằng tổng số tiền rút gốc và số tiền rút lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_KhongBangSoDu: return "M_ResponseMessage_SoTien_KhongBangSoDu";//Số tiền rút gốc không bằng số dư
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_BangSoDu: return "M_ResponseMessage_SoTien_BangSoDu";//Số tiền rút gốc bằng số dư
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_TraLaiNhoHonBangLaiTinhDuoc: return "M_ResponseMessage_SoTien_TraLaiNhoHonBangLaiTinhDuoc";//Số tiền trả lãi nhỏ hơn bằng số tiền lãi tính được
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_TraLaiLonHonLaiTinhDuoc: return "M_ResponseMessage_SoTien_TraLaiLonHonLaiTinhDuoc";//Số tiền trả lãi lớn hơn bằng số tiền lãi tính được
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_PhanBoTonTai: return "M_ResponseMessage_SoTien_PhanBoTonTai";//Số tiền phân bổ tồn tài
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_PhanBoKhongTonTai: return "M_ResponseMessage_SoTien_PhanBoKhongTonTai";//Số tiền phân bổ không tồn tài
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_PhanBoLonHonTongTienTraLai: return "M_ResponseMessage_SoTien_PhanBoLonHonTongTienTraLai";//Số tiền phân bổ lớn hơn tổng tiền trả lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_PhanBoNhoHonBangTongTienTraLai: return "M_ResponseMessage_SoTien_PhanBoNhoHonBangTongTienTraLai";//Số tiền phân bổ nhỏ hơn bằng tổng tiền trả lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_LaiNhapGocLonHonLaiDenNgay: return "M_ResponseMessage_SoTien_LaiNhapGocLonHonLaiDenNgay";//Số tiền lãi nhập gốc lớn hơn lãi tính đến ngày
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_LaiNhapGocNhoHonBangLaiDenNgay: return "M_ResponseMessage_SoTien_LaiNhapGocNhoHonBangLaiDenNgay";//Số tiền lãi nhập gốc nhỏ hơn bằng lãi tính đến ngày
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_NhoHonSoToiThieu: return "M_ResponseMessage_SoTien_NhoHonSoToiThieu";//Số tiền nhỏ hơn số tối thiểu
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_LonHonSoToiDa: return "M_ResponseMessage_SoTien_LonHonSoToiDa";//Số tiền lớn hơn số tối đa
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_HoanDuThuKhongHopLe: return "M_ResponseMessage_SoTien_HoanDuThuKhongHopLe";//Số tiền hoàn dự thu không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTien_SoTKBBNhoHon10PhanTramDuNoConLai: return "M_ResponseMessage_SoTien_SoTKBBNhoHon10PhanTramDuNoConLai";//Số dư của sổ TKBB nhỏ hơn 10% dư nợ còn lại của khoản vay

                //Số dư
                case NghiepVuResponseMessage.M_ResponseMessage_SoDu_ConLaiNhoHonSoDuQuyDinhTrongSanPham: return "M_ResponseMessage_SoDu_ConLaiNhoHonSoDuQuyDinhTrongSanPham";// Số dư còn lại nhỏ hơn số dư tối thiểu quy định ở sản phầm

                //Lãi suất
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_NhoHonKhong: return "M_ResponseMessage_LaiSuat_NhoHonKhong";//Lãi suất nhỏ hơn không
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_KhongHopLe: return "M_ResponseMessage_LaiSuat_KhongHopLe";//Không hợp lê
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_HopLe: return "M_ResponseMessage_LaiSuat_HopLe";//hợp lê
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_KhongTonTai: return "M_ResponseMessage_LaiSuat_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_KhongDuocPhepHoatDong: return "M_ResponseMessage_LaiSuat_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_LaiSuat_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_DuocPhepGiaoDich: return "M_ResponseMessage_LaiSuat_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_DonViTinhKhongHopLe: return "M_ResponseMessage_LaiSuat_DonViTinhKhongHopLe";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_NgayApDungKhongHopLe: return "M_ResponseMessage_LaiSuat_NgayApDungKhongHopLe";//Ngày áp dụng không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuat_KhongThucQuanLyCuaDonVi: return "M_ResponseMessage_LaiSuat_KhongThucQuanLyCuaDonVi";//Lãi suất không thuộc quản lý của đơn vị đăng nhập

                //Lãi suất quá hạn
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatQuanHan_KhongHopLe: return "M_ResponseMessage_LaiSuatQuanHan_KhongHopLe";//Không hợp lê
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatQuanHan_NhoHonLaiQuaHanToiDa: return "M_ResponseMessage_LaiSuatQuanHan_NhoHonLaiQuaHanToiDa";//Nhỏ hơn bằng lãi quá hạn tối đa
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatQuanHan_LonHonLaiQuaHanToiDa: return "M_ResponseMessage_LaiSuatQuanHan_LonHonLaiQuaHanToiDa";//Lớn hơn lãi quá hạn tối đa

                //Lãi suất cơ cấu lại kỳ hạn hạn trả nợ
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatCoCau_KhongHopLe: return "M_ResponseMessage_LaiSuatCoCau_KhongHopLe";//Không hợp lê

                //Cơ sở tính lãi
                case NghiepVuResponseMessage.M_ResponseMessage_CoSoTinhLai_KhongTonTai: return "M_ResponseMessage_CoSoTinhLai_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_CoSoTinhLai_KhongHopLe: return "M_ResponseMessage_CoSoTinhLai_KhongHopLe"; //Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_CoSoTinhLai_KhongDuocPhepHoatDong: return "M_ResponseMessage_CoSoTinhLai_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_CoSoTinhLai_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_CoSoTinhLai_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_CoSoTinhLai_DuocPhepGiaoDich: return "M_ResponseMessage_CoSoTinhLai_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_CoSoTinhLai_MauSoKhongHopLe: return "M_ResponseMessage_CoSoTinhLai_MauSoKhongHopLe"; //mẫu số không hợp lệ

                //Loại giao dịch phong tỏa
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGDPhongToa_KhongTonTai: return "M_ResponseMessage_LoaiGDPhongToa_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGDPhongToa_KhongDuocPhepHoatDong: return "M_ResponseMessage_LoaiGDPhongToa_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGDPhongToa_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_LoaiGDPhongToa_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiGDPhongToa_DuocPhepGiaoDich: return "M_ResponseMessage_LoaiGDPhongToa_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Phương thức phong tỏa
                case NghiepVuResponseMessage.M_ResponseMessage_PhuongThucPhongToa_KhongTonTai: return "M_ResponseMessage_PhuongThucPhongToa_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_PhuongThucPhongToa_KhongDuocPhepHoatDong: return "M_ResponseMessage_PhuongThucPhongToa_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_PhuongThucPhongToa_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_PhuongThucPhongToa_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_PhuongThucPhongToa_DuocPhepGiaoDich: return "M_ResponseMessage_PhuongThucPhongToa_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Loại nguồn vốn giải ngân
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiNguonVon_KhongTonTai: return "M_ResponseMessage_LoaiNguonVon_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiNguonVon_KhongDuocPhepHoatDong: return "M_ResponseMessage_LoaiNguonVon_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiNguonVon_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_LoaiNguonVon_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiNguonVon_DuocPhepGiaoDich: return "M_ResponseMessage_LoaiNguonVon_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Nguyên nhân quá hạn
                case NghiepVuResponseMessage.M_ResponseMessage_NguyenNhanQuaHan_KhongTonTai: return "M_ResponseMessage_NguyenNhanQuaHan_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_NguyenNhanQuaHan_KhongDuocPhepHoatDong: return "M_ResponseMessage_NguyenNhanQuaHan_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_NguyenNhanQuaHan_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_NguyenNhanQuaHan_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NguyenNhanQuaHan_DuocPhepGiaoDich: return "M_ResponseMessage_NguyenNhanQuaHan_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                //Phong tỏa - giải tỏa
                case NghiepVuResponseMessage.M_ResponseMessage_PhongToa_LonHonSoDu: return "M_ResponseMessage_PhongToa_LonHonSoDu";//Số tiền phong tỏa lớn hơn số dư
                case NghiepVuResponseMessage.M_ResponseMessage_PhongToa_NhonHonBangSoDu: return "M_ResponseMessage_PhongToa_NhonHonBangSoDu";//Số tiền phong tỏa nhỏ hơn hoặc bằng số dư

                //Hạn mức
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_KhongTonTai: return "M_ResponseMessage_HanMuc_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_KhongDuocPhepHoatDong: return "M_ResponseMessage_HanMuc_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_HanMuc_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_DuocPhepGiaoDich: return "M_ResponseMessage_HanMuc_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_HetHieuLuc: return "M_ResponseMessage_HanMuc_HetHieuLuc";//hết hiệu lực
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_HieuLuc: return "M_ResponseMessage_HanMuc_HieuLuc";//Đang hiệu lực
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_KhongThuocKhachHang: return "M_ResponseMessage_HanMuc_KhongThuocKhachHang";//không thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_ThuocKhachHang: return "M_ResponseMessage_HanMuc_ThuocKhachHang";//thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_VuotHanMucChoPhep: return "M_ResponseMessage_HanMuc_VuotHanMucChoPhep";//thuộc khách hàng

                //Loại tiền
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiTien_KhongHopLe: return "M_ResponseMessage_LoaiTien_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiTien_TonTai: return "M_ResponseMessage_LoaiTien_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiTien_KhongTonTai: return "M_ResponseMessage_LoaiTien_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiTien_KhongDuocPhepHoatDong: return "M_ResponseMessage_LoaiTien_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiTien_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_LoaiTien_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_LoaiTien_DuocPhepGiaoDich: return "M_ResponseMessage_LoaiTien_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Tính chất Mã loại tài khoản (nội bảng - ngoại bảng)
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTaiKhoan_TonTai: return "M_ResponseMessage_MaLoaiTaiKhoan_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTaiKhoan_KhongTonTai: return "M_ResponseMessage_MaLoaiTaiKhoan_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTaiKhoan_KhongDuocPhepHoatDong: return "M_ResponseMessage_MaLoaiTaiKhoan_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTaiKhoan_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_MaLoaiTaiKhoan_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTaiKhoan_DuocPhepGiaoDich: return "M_ResponseMessage_MaLoaiTaiKhoan_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Tính chất Mã loại khách hàng nội bộ- khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiKHangNBo_TonTai: return "M_ResponseMessage_MaLoaiKHangNBo_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiKHangNBo_KhongTonTai: return "M_ResponseMessage_MaLoaiKHangNBo_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiKHangNBo_KhongDuocPhepHoatDong: return "M_ResponseMessage_MaLoaiKHangNBo_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiKHangNBo_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_MaLoaiKHangNBo_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiKHangNBo_DuocPhepGiaoDich: return "M_ResponseMessage_MaLoaiKHangNBo_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Tính chất Mã loại thu nhap - chi phi
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTNhapCPhi_TonTai: return "M_ResponseMessage_MaLoaiTNhapCPhi_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTNhapCPhi_KhongTonTai: return "M_ResponseMessage_MaLoaiTNhapCPhi_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTNhapCPhi_KhongDuocPhepHoatDong: return "M_ResponseMessage_MaLoaiTNhapCPhi_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTNhapCPhi_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_MaLoaiTNhapCPhi_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_MaLoaiTNhapCPhi_DuocPhepGiaoDich: return "M_ResponseMessage_MaLoaiTNhapCPhi_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Tính chất số dư
                case NghiepVuResponseMessage.M_ResponseMessage_TinhChatSoDu_KhongHopLe: return "M_ResponseMessage_TinhChatSoDu_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TinhChatSoDu_TonTai: return "M_ResponseMessage_TinhChatSoDu_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_TinhChatSoDu_KhongTonTai: return "M_ResponseMessage_TinhChatSoDu_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_TinhChatSoDu_KhongDuocPhepHoatDong: return "M_ResponseMessage_TinhChatSoDu_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TinhChatSoDu_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TinhChatSoDu_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TinhChatSoDu_DuocPhepGiaoDich: return "M_ResponseMessage_TinhChatSoDu_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Phân loại tài khoản
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_DauVaoKhongHopLe: return "M_ResponseMessage_PhanLoaiTaiKhoan_DauVaoKhongHopLe";//Đầu vào không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_TonTai: return "M_ResponseMessage_PhanLoaiTaiKhoan_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_KhongTonTai: return "M_ResponseMessage_PhanLoaiTaiKhoan_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_KhongDuocPhepHoatDong: return "M_ResponseMessage_PhanLoaiTaiKhoan_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_PhanLoaiTaiKhoan_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_DuocPhepGiaoDich: return "M_ResponseMessage_PhanLoaiTaiKhoan_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_TenKhongHopLe: return "M_ResponseMessage_PhanLoaiTaiKhoan_TenKhongHopLe";//Tên phân loại tài khoản không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_MaKhongHopLe: return "M_ResponseMessage_PhanLoaiTaiKhoan_MaKhongHopLe";//Mã phân loại tài khoản không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_MaChaDaCoTaiKhoanChiTiet: return "M_ResponseMessage_PhanLoaiTaiKhoan_MaChaDaCoTaiKhoanChiTiet"; //Mã phân loại cha đã có tài khoản chi tiết
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_KhongThuocQuanLyCuaDonVi: return "M_ResponseMessage_PhanLoaiTaiKhoan_KhongThuocQuanLyCuaDonVi"; //Mã phân loại không thuộc quản lý của đơn vị                
                //Phân loại tài khoản
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_KhongNhoNhat: return "M_ResponseMessage_PhanLoaiTaiKhoan_KhongNhoNhat";//Đầu vào không hợp lệ

                //Tài khoản
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_TenKhongHopLe: return "M_ResponseMessage_TaiKhoan_TenKhongHopLe";//Tên tài khoản không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_DauVaoKhongHopLe: return "M_ResponseMessage_TaiKhoan_DauVaoKhongHopLe";//Đầu vào không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongHopLe: return "M_ResponseMessage_TaiKhoan_KhongHopLe";//Tài khoản không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_TonTai: return "M_ResponseMessage_TaiKhoan_TonTai";//Tài khoản đang tồn tai
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongTonTai: return "M_ResponseMessage_TaiKhoan_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongDuocPhepHoatDong: return "M_ResponseMessage_TaiKhoan_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TaiKhoan_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_DuocPhepGiaoDich: return "M_ResponseMessage_TaiKhoan_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNoiBo: return "M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNoiBo";//không phải là tài khoản nội bộ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_LaTaiKhoanNoiBo: return "M_ResponseMessage_TaiKhoan_LaTaiKhoanNoiBo";//là tài khoản nội bộ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanKhachHang: return "M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanKhachHang";//không phải là tài khoản khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_LaTaiKhoanKhachHang: return "M_ResponseMessage_TaiKhoan_LaTaiKhoanKhachHang";//là tài khoản khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNoiBang: return "M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNoiBang";//không phải là tài khoản nội bảng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_LaTaiKhoanNoiBang: return "M_ResponseMessage_TaiKhoan_LaTaiKhoanNoiBang";//là tài khoản nội bảng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNgoaiBang: return "M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanNgoaiBang";//không phải là tài khoản ngoại bảng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_LaTaiKhoanNgoaiBang: return "M_ResponseMessage_TaiKhoan_LaTaiKhoanNgoaiBang";//là tài khoản ngoại bảng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanThuNhap: return "M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanThuNhap";//không phải là tài khoản thu nhập
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_LaTaiKhoanThuNhap: return "M_ResponseMessage_TaiKhoan_LaTaiKhoanThuNhap";//là tài khoản thu nhập
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanChiPhi: return "M_ResponseMessage_TaiKhoan_KhongLaTaiKhoanChiPhi";//không phải là tài khoản chi phí
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_LaTaiKhoanChiPhi: return "M_ResponseMessage_TaiKhoan_LaTaiKhoanChiPhi";//là tài khoản chi phí
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongDuSoDuKhaDung: return "M_ResponseMessage_TaiKhoan_KhongDuSoDuKhaDung";//Không đủ số dư khả dụng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongDuSoDu: return "M_ResponseMessage_TaiKhoan_KhongDuSoDu";//Không đủ số dư
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_ChuaDong: return "M_ResponseMessage_TaiKhoan_ChuaDong";//Tài khoản chưa đóng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_DangDong: return "M_ResponseMessage_TaiKhoan_DangDong";//Tài khoản đang đóng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_TongNoBangTongCo: return "M_ResponseMessage_TaiKhoan_TongNoBangTongCo";//Tổng nợ bằng tổng có
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_TongNoLonHonTongCo: return "M_ResponseMessage_TaiKhoan_TongNoLonHonTongCo";//Tổng nợ lớn hơn tổng có
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_TongNoNhoHonTongCo: return "M_ResponseMessage_TaiKhoan_TongNoNhoHonTongCo";//Tổng nợ nhỏ hơn tổng có
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongThuocKhachHang: return "M_ResponseMessage_TaiKhoan_KhongThuocKhachHang"; //Không thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_ThuocKhachHang: return "M_ResponseMessage_TaiKhoan_ThuocKhachHang"; //Thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongThuocDonVi: return "M_ResponseMessage_TaiKhoan_KhongThuocDonVi"; //Không thuộc đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_ThuocDonVi: return "M_ResponseMessage_TaiKhoan_ThuocDonVi"; //Thuộc đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_NgayPhongToaKhongHopLe: return "M_ResponseMessage_TaiKhoan_NgayPhongToaKhongHopLe"; //Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_NgayPhongToaHopLe: return "M_ResponseMessage_TaiKhoan_NgayPhongToaHopLe"; //hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_NgayDongKhongHopLe: return "M_ResponseMessage_TaiKhoan_NgayDongKhongHopLe"; //Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_NgayDongHopLe: return "M_ResponseMessage_TaiKhoan_NgayDongHopLe"; //hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_NgayMoKhongHopLe: return "M_ResponseMessage_TaiKhoan_NgayMoKhongHopLe"; //Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_NgayMoHopLe: return "M_ResponseMessage_TaiKhoan_NgayMoHopLe"; //hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_ChuaDangKy: return "M_ResponseMessage_TaiKhoan_ChuaDangKy"; //Chưa đăng ký
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_PhanLoaiKhongHopLe: return "M_ResponseMessage_TaiKhoan_PhanLoaiKhongHopLe"; //Phân loại tài khoản không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoan_KhongThuocQuanLyCuaDonVi: return "M_ResponseMessage_TaiKhoan_KhongThuocQuanLyCuaDonVi"; //Tài khoản không thuộc quản lý của đơn vị                    

                //Tài khoản khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_KhongTonTai: return "M_ResponseMessage_TaiKhoanKhachHang_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_KhongDuocPhepHoatDong: return "M_ResponseMessage_TaiKhoanKhachHang_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TaiKhoanKhachHang_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_DuocPhepGiaoDich: return "M_ResponseMessage_TaiKhoanKhachHang_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_DangBiDong: return "M_ResponseMessage_TaiKhoanKhachHang_DangBiDong";//Đang bị đóng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_DangBiPhongToa: return "M_ResponseMessage_TaiKhoanKhachHang_DangBiPhongToa";//Đang bị phong tỏa
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_KhongDuSoDu: return "M_ResponseMessage_TaiKhoanKhachHang_KhongDuSoDu";//không đủ sổ dư
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_KhongDuSoDuKhaDung: return "M_ResponseMessage_TaiKhoanKhachHang_KhongDuSoDuKhaDung";//không đủ sổ dư khả dụng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanKhachHang_KhongThuocCSH: return "M_ResponseMessage_TaiKhoanKhachHang_KhongThuocCSH";//không thuộc khách hàng, khách hàng không là chủ sở hữu

                //Tài khoản nội bộ
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_KhongTonTai: return "M_ResponseMessage_TaiKhoanNoiBo_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_KhongDuocPhepHoatDong: return "M_ResponseMessage_TaiKhoanNoiBo_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TaiKhoanNoiBo_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_DuocPhepGiaoDich: return "M_ResponseMessage_TaiKhoanNoiBo_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_DangBiDong: return "M_ResponseMessage_TaiKhoanNoiBo_DangBiDong";//Đang bị đóng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_DangBiPhongToa: return "M_ResponseMessage_TaiKhoanNoiBo_DangBiPhongToa";//Đang bị phong toa
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_KhongDuSoDu: return "M_ResponseMessage_TaiKhoanNoiBo_KhongDuSoDu";//không đủ số dư
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_KhongDuSoDuKhaDung: return "M_ResponseMessage_TaiKhoanNoiBo_KhongDuSoDuKhaDung";//không đủ sổ dư khả dụng
                case NghiepVuResponseMessage.M_ResponseMessage_TaiKhoanNoiBo_KhongThuocCSH: return "M_ResponseMessage_TaiKhoanNoiBo_KhongThuocCSH";//không thuộc đơn vị, khách hàng không là chủ sở hữu

                //Kế hoạch
                case NghiepVuResponseMessage.M_ResponseMessage_KeHoach_ThemKhongThanhCong: return "M_ResponseMessage_KeHoach_ThemKhongThanhCong"; // Thêm không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KeHoach_ThemThanhCong: return "M_ResponseMessage_KeHoach_ThemThanhCong"; //Thêm thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KeHoach_SuaKhongThanhCong: return "M_ResponseMessage_KeHoach_SuaKhongThanhCong"; //Sửa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KeHoach_SuaThanhCong: return "M_ResponseMessage_KeHoach_SuaThanhCong"; //Sửa thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KeHoach_XoaKhongThanhCong: return "M_ResponseMessage_KeHoach_XoaKhongThanhCong"; //Xóa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_KeHoach_XoaThanhCong: return "M_ResponseMessage_KeHoach_XoaThanhCong"; //Xóa thành công

                //Từ ngày, đến ngày
                case NghiepVuResponseMessage.M_ResponseMessage_TuDen_TuNgayLonHonDenNgay: return "M_ResponseMessage_TuDen_TuNgayLonHonDenNgay";//Từ ngày lớn hơn đến ngày
                case NghiepVuResponseMessage.M_ResponseMessage_TuDen_TuNgayNhoHonBangDenNgay: return "M_ResponseMessage_TuDen_TuNgayNhoHonBangDenNgay";//Từ ngày nhỏ hơn bằng đến ngày

                //Ngày dự kiến phát vốn
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuKienPhatVon_KhongHopLe: return "M_ResponseMessage_NgayDuKienPhatVon_KhongHopLe"; //Ngày lập không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuKienPhatVon_HopLe: return "M_ResponseMessage_NgayDuKienPhatVon_HopLe"; //Ngày lập hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuKienPhatVon_BangNgayGiaoDich: return "M_ResponseMessage_NgayDuKienPhatVon_BangNgayGiaoDich";  //Ngày lập bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuKienPhatVon_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayDuKienPhatVon_NhoHonNgayGiaoDich";  //Ngày lập nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuKienPhatVon_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayDuKienPhatVon_LonHonNgayGiaoDich";  //Ngày lập lớn hơn ngày giao dịch

                //Ngày làm việc
                case NghiepVuResponseMessage.M_ResponseMessage_NgayLamViec_HopLe: return "M_ResponseMessage_NgayLamViec_HopLe"; //Ngày làm việc hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayLamViec_KhongHopLe: return "M_ResponseMessage_NgayLamViec_KhongHopLe"; //Ngày làm việc không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayLamViec_KhongKhongTonTai: return "M_ResponseMessage_NgayLamViec_KhongKhongTonTai"; //Ngày làm việc không tồn tại

                //Gia hạn nợ
                case NghiepVuResponseMessage.M_ResponseMessage_GiaHan_GocKhongHopLe: return "M_ResponseMessage_GiaHan_GocKhongHopLe"; //Gia han gốc không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_GiaHan_GocHopLe: return "M_ResponseMessage_GiaHan_GocHopLe"; //Gia han gốc hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_GiaHan_LaiKhongHopLe: return "M_ResponseMessage_GiaHan_LaiKhongHopLe"; //Gia han lãi không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_GiaHan_LaiHopLe: return "M_ResponseMessage_GiaHan_LaiHopLe"; //Gia han lãi hợp lệ

                //Ngày gia hạn
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaHan_KhongHopLe: return "M_ResponseMessage_NgayGiaHan_KhongHopLe"; //Ngày gia han không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaHan_BangNgayGiaoDich: return "M_ResponseMessage_NgayGiaHan_BangNgayGiaoDich"; //Ngày gia han bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaHan_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayGiaHan_NhoHonNgayGiaoDich"; //Ngày gia han nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaHan_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayGiaHan_LonHonNgayGiaoDich"; //Ngày gia han lớn hơn ngày giao dịch

                //Ngày thu
                case NghiepVuResponseMessage.M_ResponseMessage_NgayThu_KhongHopLe: return "M_ResponseMessage_NgayThu_KhongHopLe"; //Ngày thu không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayThu_BangNgayGiaoDich: return "M_ResponseMessage_NgayThu_BangNgayGiaoDich"; //Ngày thu bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayThu_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayThu_NhoHonNgayGiaoDich"; //Ngày thu nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayThu_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayThu_LonHonNgayGiaoDich"; //Ngày thu lớn hơn ngày giao dịch

                //Ngày tra
                case NghiepVuResponseMessage.M_ResponseMessage_NgayTra_LonHonNgayDangKy: return "M_ResponseMessage_NgayTra_LonHonNgayDangKy"; //Ngày trả phải từ 15 ngày trờ lên kể từ ngày đăng ký

                //Ngày áp dụng
                case NghiepVuResponseMessage.M_ResponseMessage_NgayApDung_KhongHopLe: return "M_ResponseMessage_NgayApDung_KhongHopLe";//Ngày lập không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayApDung_BangNgayGiaoDich: return "M_ResponseMessage_NgayApDung_BangNgayGiaoDich";//Ngày lập bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayApDung_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayApDung_NhoHonNgayGiaoDich";//Ngày lập nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayApDung_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayApDung_LonHonNgayGiaoDich";//Ngày lập lớn hơn ngày giao dịch

                //Ngày hợp đồng
                case NghiepVuResponseMessage.M_ResponseMessage_NgayHopDong_KhongHopLe: return "M_ResponseMessage_NgayHopDong_KhongHopLe";//Ngày hợp đồng không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayHopDong_BangNgayGiaoDich: return "M_ResponseMessage_NgayHopDong_BangNgayGiaoDich";//Ngày hợp đồng bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayHopDong_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayHopDong_NhoHonNgayGiaoDich";//Ngày hợp đồng nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayHopDong_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayHopDong_LonHonNgayGiaoDich";//Ngày hợp đồng lớn hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayHopDong_NhoHonNgayKhangThamGia: return "M_ResponseMessage_NgayHopDong_NhoHonNgayKhangThamGia";//Ngày hợp đồng nhỏ hơn ngày khách hàng tham gia

                //Ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaoDich_KhongHopLe: return "M_ResponseMessage_NgayGiaoDich_KhongHopLe";//Ngày lập không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaoDich_BangNgayGiaoDich: return "M_ResponseMessage_NgayGiaoDich_BangNgayGiaoDich";//Ngày lập bằng ngày làm việc
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaoDich_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayGiaoDich_NhoHonNgayGiaoDich";//Ngày lập nhỏ hơn ngày làm việc
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaoDich_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayGiaoDich_LonHonNgayGiaoDich";//Ngày lập lớn hơn ngày làm việc

                //Ngày mở sổ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayMoSo_KhongHopLe: return "M_ResponseMessage_NgayMoSo_KhongHopLe";// không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayMoSo_BangNgayGiaoDich: return "M_ResponseMessage_NgayMoSo_BangNgayGiaoDich";//bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayMoSo_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayMoSo_NhoHonNgayGiaoDich";//nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayMoSo_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayMoSo_LonHonNgayGiaoDich";//lớn hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayMoSo_KhachNgayGiaoDich: return "M_ResponseMessage_NgayMoSo_KhachNgayGiaoDich";//khác ngày giao dịch


                //Ngày lập
                case NghiepVuResponseMessage.M_ResponseMessage_NgayLap_KhongHopLe: return "M_ResponseMessage_NgayLap_KhongHopLe";//Ngày lập không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayLap_BangNgayGiaoDich: return "M_ResponseMessage_NgayLap_BangNgayGiaoDich";//Ngày lập bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayLap_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayLap_NhoHonNgayGiaoDich";//Ngày lập nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayLap_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayLap_LonHonNgayGiaoDich";//Ngày lập lớn hơn ngày giao dịch

                //Ngày cập nhật
                case NghiepVuResponseMessage.M_ResponseMessage_NgayCapNhat_KhongHopLe: return "M_ResponseMessage_NgayCapNhat_KhongHopLe";//Ngày cập nhật không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayCapNhat_BangNgayGiaoDich: return "M_ResponseMessage_NgayCapNhat_BangNgayGiaoDich";//Ngày cập nhật bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayCapNhat_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayCapNhat_NhoHonNgayGiaoDich";//Ngày cập nhật nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayCapNhat_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayCapNhat_LonHonNgayGiaoDich";//Ngày cập nhật lớn hơn ngày giao dịch

                //Ngày giải ngân
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaiNgan_HopLe: return "M_ResponseMessage_NgayGiaiNgan_HopLe";//hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaiNgan_KhongHopLe: return "M_ResponseMessage_NgayGiaiNgan_KhongHopLe";//không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaiNgan_BangNgayGiaoDich: return "M_ResponseMessage_NgayGiaiNgan_BangNgayGiaoDich";//bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaiNgan_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayGiaiNgan_NhoHonNgayGiaoDich";//nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayGiaiNgan_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayGiaiNgan_LonHonNgayGiaoDich";//lớn hơn ngày giao dịch

                //Dự thu đến ngày
                case NghiepVuResponseMessage.M_ResponseMessage_DuThuDenNgay_HopLe: return "M_ResponseMessage_DuThuDenNgay_HopLe";//hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_DuThuDenNgay_KhongHopLe: return "M_ResponseMessage_DuThuDenNgay_KhongHopLe";//không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_DuThuDenNgay_BangNgayGiaoDich: return "M_ResponseMessage_DuThuDenNgay_BangNgayGiaoDich";//bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_DuThuDenNgay_NhoHonNgayGiaoDich: return "M_ResponseMessage_DuThuDenNgay_NhoHonNgayGiaoDich";//nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_DuThuDenNgay_LonHonNgayGiaoDich: return "M_ResponseMessage_DuThuDenNgay_LonHonNgayGiaoDich";//lớn hơn ngày giao dịch

                //Ngày tính lãi
                case NghiepVuResponseMessage.M_ResponseMessage_NgayTinhLai_HopLe: return "M_ResponseMessage_NgayTinhLai_HopLe";//hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayTinhLai_KhongHopLe: return "M_ResponseMessage_NgayTinhLai_KhongHopLe";//không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayTinhLai_BangNgayGiaoDich: return "M_ResponseMessage_NgayTinhLai_BangNgayGiaoDich";//bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayTinhLai_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayTinhLai_NhoHonNgayGiaoDich"; //nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayTinhLai_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayTinhLai_LonHonNgayGiaoDich";//lớn hơn ngày giao dịch

                //Ngày dự chi
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuChi_HopLe: return "M_ResponseMessage_NgayDuChi_HopLe";              //hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuChi_KhongHopLe: return "M_ResponseMessage_NgayDuChi_KhongHopLe";         //không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuChi_BangNgayGiaoDich: return "M_ResponseMessage_NgayDuChi_BangNgayGiaoDich";   //bằng ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuChi_NhoHonNgayGiaoDich: return "M_ResponseMessage_NgayDuChi_NhoHonNgayGiaoDich"; //nhỏ hơn ngày giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDuChi_LonHonNgayGiaoDich: return "M_ResponseMessage_NgayDuChi_LonHonNgayGiaoDich"; //lớn hơn ngày giao dịch

                //Người lập
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiLap_KhongTonTai: return "M_ResponseMessage_NguoiLap_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiLap_KhongDuocPhepHoatDong: return "M_ResponseMessage_NguoiLap_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiLap_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_NguoiLap_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiLap_DuocPhepGiaoDich: return "M_ResponseMessage_NguoiLap_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Người cập nhật
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiCapNhat_KhongTonTai: return "M_ResponseMessage_NguoiCapNhat_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiCapNhat_KhongDuocPhepHoatDong: return "M_ResponseMessage_NguoiCapNhat_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiCapNhat_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_NguoiCapNhat_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiCapNhat_DuocPhepGiaoDich: return "M_ResponseMessage_NguoiCapNhat_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Người sử dụng
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_KhongTonTai: return "M_ResponseMessage_NguoiSuDung_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_KhongDuocPhepHoatDong: return "M_ResponseMessage_NguoiSuDung_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_NguoiSuDung_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_DuocPhepGiaoDich: return "M_ResponseMessage_NguoiSuDung_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_ChuaDuocPhepThemMoi: return "M_ResponseMessage_NguoiSuDung_ChuaDuocPhepThemMoi";//chưa được phép thêm mới
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_ChuaDuocPhepSua: return "M_ResponseMessage_NguoiSuDung_ChuaDuocPhepSua";//chưa được phép sửa
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_ChuaDuocPhepXoa: return "M_ResponseMessage_NguoiSuDung_ChuaDuocPhepXoa";//chưa được phép xóa
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_ChuaDuocPhepDuyet: return "M_ResponseMessage_NguoiSuDung_ChuaDuocPhepDuyet";//chưa được phép duyệt
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_ChuaDuocPhepThoaiDuyet: return "M_ResponseMessage_NguoiSuDung_ChuaDuocPhepThoaiDuyet";//chưa được phép thoái duyệt
                case NghiepVuResponseMessage.M_ResponseMessage_NguoiSuDung_ChuaDuocPhepTuChoiDuyet: return "M_ResponseMessage_NguoiSuDung_ChuaDuocPhepTuChoiDuyet";//chưa được phép từ chối duyệt

                //Sổ tiết kiệm
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TonTai: return "M_ResponseMessage_SoTietKiem_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KhongTonTai: return "M_ResponseMessage_SoTietKiem_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KhongHopLe: return "M_ResponseMessage_SoTietKiem_KhongHopLe";//Không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KhongDuocPhepHoatDong: return "M_ResponseMessage_SoTietKiem_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_SoTietKiem_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DuocPhepGiaoDich: return "M_ResponseMessage_SoTietKiem_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_NgayApDungKhongHopLe: return "M_ResponseMessage_SoTietKiem_NgayApDungKhongHopLe";//ngày áp dụng không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_NgayMoSoKhongHopLe: return "M_ResponseMessage_SoTietKiem_NgayMoSoKhongHopLe";//ngày mở sổ không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_NgayDaoHanKhongHopLe: return "M_ResponseMessage_SoTietKiem_NgayDaoHanKhongHopLe";//ngày đáo hạn không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TienMatKhongHopLe: return "M_ResponseMessage_SoTietKiem_TienMatKhongHopLe";//tiền mặt không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SoTienBangTienMatKhongHopLe: return "M_ResponseMessage_SoTietKiem_SoTienBangTienMatKhongHopLe";//số tiền bằng tiền mặt không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SoTienBangChuyenKhoanKhongHopLe: return "M_ResponseMessage_SoTietKiem_SoTienBangChuyenKhoanKhongHopLe";//số tiền bằng chuyển khoản không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_NhoHonSoToiThieu: return "M_ResponseMessage_SoTietKiem_NhoHonSoToiThieu";//số tiền mở sổ nhỏ hơn số tiền tối thiểu được quy định theo sản phẩm
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_LonHonSoToiDa: return "M_ResponseMessage_SoTietKiem_LonHonSoToiDa";//số tiền mở sổ lớn hơn số tiền tối đa được quy định theo sản phẩm
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DaTatToan: return "M_ResponseMessage_SoTietKiem_DaTatToan";//số tiền mở hoặc tài khoản đã tất toán hoặc đóng
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ChuaTatToan: return "M_ResponseMessage_SoTietKiem_ChuaTatToan";//số tiền mở hoặc tài khoản chưa tất toán hoặc đóng
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KhongThuocDonVi: return "M_ResponseMessage_SoTietKiem_KhongThuocDonVi";//Sổ tiết kiệm không thuộc đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KhongThuocPhongGiaoDich: return "M_ResponseMessage_SoTietKiem_KhongThuocPhongGiaoDich";//số tiết kiệm không thuộc phòng giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThuocDonVi: return "M_ResponseMessage_SoTietKiem_ThuocDonVi";//số tiền mở thuộc đơn vị
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SoTienRutGocNhoHonKhong: return "M_ResponseMessage_SoTietKiem_SoTienRutGocNhoHonKhong";//Nhỏ hơn không
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SoTienRutGocLonHonSoDuKhaDung: return "M_ResponseMessage_SoTietKiem_SoTienRutGocLonHonSoDuKhaDung";//Lớn hơn số dư khả dụng
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SoTienRutGocNhoHonBangSoDuKhaDung: return "M_ResponseMessage_SoTietKiem_SoTienRutGocNhoHonBangSoDuKhaDung";//lớn hơn 0 và nhỏ hơn bằng số dư khả dụng
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_XoaThanhCong: return "M_ResponseMessage_SoTietKiem_XoaThanhCong";//Xóa thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DuyetThanhCong: return "M_ResponseMessage_SoTietKiem_DuyetThanhCong";//Duyệt thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThoaiDuyetThanhCong: return "M_ResponseMessage_SoTietKiem_ThoaiDuyetThanhCong";//Thoái duyệt thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TuChoiDuyetThanhCong: return "M_ResponseMessage_SoTietKiem_TuChoiDuyetThanhCong";//Từ chối duyệt thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_XoaKhongThanhCong: return "M_ResponseMessage_SoTietKiem_XoaKhongThanhCong";//Xóa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DuyetKhongThanhCong: return "M_ResponseMessage_SoTietKiem_DuyetKhongThanhCong";//Duyệt không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThoaiDuyetKhongThanhCong: return "M_ResponseMessage_SoTietKiem_ThoaiDuyetKhongThanhCong";//Thoái duyệt không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TuChoiDuyetKhongThanhCong: return "M_ResponseMessage_SoTietKiem_TuChoiDuyetKhongThanhCong";//Từ chối duyệt không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SuaThanhCong: return "M_ResponseMessage_SoTietKiem_SuaThanhCong";//Sửa thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SuaKhongThanhCong: return "M_ResponseMessage_SoTietKiem_SuaKhongThanhCong";//Sửa không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThemThanhCong: return "M_ResponseMessage_SoTietKiem_ThemThanhCong";//Thêm thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThemKhongThanhCong: return "M_ResponseMessage_SoTietKiem_ThemKhongThanhCong";//Thêm không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TaoThongTinLichSuKhongThanhCong: return "M_ResponseMessage_SoTietKiem_TaoThongTinLichSuKhongThanhCong"; //Tạo thông tin lịch sử ko thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_SaoLuuKhongThanhCong: return "M_ResponseMessage_SoTietKiem_SaoLuuKhongThanhCong"; //Sao lưu ko thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_XoaLichSuKhongThanhCong: return "M_ResponseMessage_SoTietKiem_XoaLichSuKhongThanhCong"; //Xoa khong thanh công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_XoaLichSuThanhCong: return "M_ResponseMessage_SoTietKiem_XoaLichSuThanhCong"; //Xoa thanh công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThemLichSuKhongThanhCong: return "M_ResponseMessage_SoTietKiem_ThemLichSuKhongThanhCong"; //Thêm khong thanh công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThemLichSuThanhCong: return "M_ResponseMessage_SoTietKiem_ThemLichSuThanhCong"; //Thêm thanh công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TinhDuChiKhongThanhCong: return "M_ResponseMessage_SoTietKiem_TinhDuChiKhongThanhCong"; //Tính dự chi không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ChuaDenHanTatToan: return "M_ResponseMessage_SoTietKiem_ChuaDenHanTatToan";//Chưa đến hạn tất toán
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TinhLaiSTKKKHKhongThanhCong: return "M_ResponseMessage_SoTietKiem_TinhLaiSTKKKHKhongThanhCong"; //Tính lãi sổ tiết kiệm không kỳ hạn không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TinhLaiSTKQDKhongThanhCong: return "M_ResponseMessage_SoTietKiem_TinhLaiSTKQDKhongThanhCong"; //Tính lãi sổ tiết kiệm quy định không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_TinhLaiSTKCKHTHKhongThanhCong: return "M_ResponseMessage_SoTietKiem_TinhLaiSTKCKHTHKhongThanhCong"; //Tính lãi sổ tiết kiệm quy định không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DaThucHienLNGDenHetThang: return "M_ResponseMessage_SoTietKiem_DaThucHienLNGDenHetThang"; //Đã thực hiện lãi nhập gốc hoặc trả lãi đến hết tháng
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ChuaDenNgayNhanLai: return "M_ResponseMessage_SoTietKiem_ChuaDenNgayNhanLai"; //Chưa đến ngày nhận lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DaThucHienLNGHoacTraLaiDenHienTai: return "M_ResponseMessage_SoTietKiem_DaThucHienLNGHoacTraLaiDenHienTai"; //Đã thực hiện lãi nhập gốc hoặc trả lãi đến thời điểm hiện tại
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DaNhanLai: return "M_ResponseMessage_SoTietKiem_DaNhanLai"; //Sổ tiết kiệm đã nhận lãi
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KoDuocTatToanTruocHan: return "M_ResponseMessage_SoTietKiem_KoDuocTatToanTruocHan"; //Sổ tiết kiệm không được tất toán trước hạn
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KoLayDuocLaiSuatTruocHan: return "M_ResponseMessage_SoTietKiem_KoLayDuocLaiSuatTruocHan"; //Không lấy được lãi suất trước hạn
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DaDuChiDenHienTai: return "M_ResponseMessage_SoTietKiem_DaDuChiDenHienTai"; //Đã thực hiện dự chi đến thời điểm hiện tại
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ChuaDenNgayLNG: return "M_ResponseMessage_SoTietKiem_ChuaDenNgayLNG"; //Chưa đến ngày thực hiện lãi nhập gốc
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ChuaThucHienNgayLNG: return "M_ResponseMessage_SoTietKiem_ChuaThucHienNgayLNG"; //Chưa thực hiện lãi nhập gốc
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DangTonTaiGiaoDichChoDuyet: return "M_ResponseMessage_SoTietKiem_DangTonTaiGiaoDichChoDuyet"; //Đang tồn tại giao dịch chờ duyệt
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DangTonTaiGiaoDichChuaDuyet: return "M_ResponseMessage_SoTietKiem_DangTonTaiGiaoDichChuaDuyet"; //Đang tồn tại giao dịch chưa duyệt
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_DaTonTaiMotSoKhacDangHoatDong: return "M_ResponseMessage_SoTietKiem_DaTonTaiMotSoKhacDangHoatDong"; //Đã tồn tại một sổ khác của khách hàng đang hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_KhongDuocPhepTatToanKhiConSoDu: return "M_ResponseMessage_SoTietKiem_KhongDuocPhepTatToanKhiConSoDu"; //Không được phép tất toán khi còn số dư  
                case NghiepVuResponseMessage.M_ResponseMessage_SoTietKiem_ThanhVienChuaDatDuSoVongVayVon: return "M_ResponseMessage_SoTietKiem_ThanhVienChuaDatDuSoVongVayVon"; //Thành viên chưa đạt đủ số vòng vay vốn                                                      
                    
                //Huy động vốn - Mở sổ tiết kiệm quy định
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_ChuaDuocKhoiTao: return "M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_ChuaDuocKhoiTao";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayLamViecKhongHopLe: return "M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayLamViecKhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayMoSoKhongHopLe: return "M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayMoSoKhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayDaoHanKhongHopLe: return "M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_NgayDaoHanKhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_HinhThucGiaoDichKhongTonTai: return "M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_HinhThucGiaoDichKhongTonTai";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_KhongDuocPhepSuaThongTin: return "M_ResponseMessage_HuyDongVon_MoSoTietKiemQuyDinh_KhongDuocPhepSuaThongTin";//Không được phép sửa thông tin

                //Sổ tiết kiệm - Tài khoản CA
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_TaiKhoanCA_ThemThanhCong: return "M_ResponseMessage_HuyDongVon_TaiKhoanCA_ThemThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_TaiKhoanCA_ThemKhongThanhCong: return "M_ResponseMessage_HuyDongVon_TaiKhoanCA_ThemKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_TaiKhoanCA_DuyetThanhCong: return "M_ResponseMessage_HuyDongVon_TaiKhoanCA_DuyetThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_HuyDongVon_TaiKhoanCA_DuyetKhongThanhCong: return "M_ResponseMessage_HuyDongVon_TaiKhoanCA_DuyetKhongThanhCong";

                //Lãi quá hạn
                case NghiepVuResponseMessage.M_ResponseMessage_LaiQuaHan_HopLe: return "M_ResponseMessage_LaiQuaHan_HopLe";//Lãi quá hạn hợp lệ, không vượt quy định của hệ thống
                case NghiepVuResponseMessage.M_ResponseMessage_LaiQuaHan_khongHopLe: return "M_ResponseMessage_LaiQuaHan_khongHopLe";//Lãi quá hạn không hợp lệ, vượt quy định của hệ thống

                //Sản phẩm tín dụng
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamTinDung_KhongTonTai: return "M_ResponseMessage_SanPhamTinDung_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamTinDung_KhongDuocPhepHoatDong: return "M_ResponseMessage_SanPhamTinDung_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamTinDung_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_SanPhamTinDung_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SanPhamTinDung_DuocPhepGiaoDich: return "M_ResponseMessage_SanPhamTinDung_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Hợp đồng thế chấp
                case NghiepVuResponseMessage.M_ResponseMessage_HDTC_KhongTonTai: return "M_ResponseMessage_HDTC_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_HDTC_KhongDuocPhepHoatDong: return "M_ResponseMessage_HDTC_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_HDTC_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_HDTC_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_HDTC_DuocPhepGiaoDich: return "M_ResponseMessage_HDTC_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Tài sản đảm bảo
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_KhongTonTai: return "M_ResponseMessage_TSDB_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_KhongDuocPhepHoatDong: return "M_ResponseMessage_TSDB_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TSDB_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_DuocPhepGiaoDich: return "M_ResponseMessage_TSDB_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_ThuocKhachHang: return "M_ResponseMessage_TSDB_ThuocKhachHang";//Tài sản thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_KhongThuocKhachHang: return "M_ResponseMessage_TSDB_KhongThuocKhachHang";//Tài sản không thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_VuotQuaSoTienDamBao: return "M_ResponseMessage_TSDB_VuotQuaSoTienDamBao";//Vượt quá số tiền có thể sử dụng để đảm bảo
                case NghiepVuResponseMessage.M_ResponseMessage_TSDB_KhongVuotQuaSoTienDamBao: return "M_ResponseMessage_TSDB_KhongVuotQuaSoTienDamBao";//Không Vượt quá số tiền có thể sử dụng để đảm bảo

                //Hợp đồng tín dụng vi mô
                case NghiepVuResponseMessage.M_ResponseMessage_HDTDVM_KhongTonTai: return "M_ResponseMessage_HDTDVM_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_HDTDVM_TonTai: return "M_ResponseMessage_HDTDVM_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_HDTDVM_KhongDuocPhepHoatDong: return "M_ResponseMessage_HDTDVM_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_HDTDVM_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_HDTDVM_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_HDTDVM_DuocPhepGiaoDich: return "M_ResponseMessage_HDTDVM_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch

                //Khế ước vi mô
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_KhongTonTai: return "M_ResponseMessage_KUOCVM_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_TonTai: return "M_ResponseMessage_KUOCVM_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_KhongDuocPhepHoatDong: return "M_ResponseMessage_KUOCVM_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_KUOCVM_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_DuocPhepGiaoDich: return "M_ResponseMessage_KUOCVM_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ConDuNo: return "M_ResponseMessage_KUOCVM_ConDuNo";//Còn dư nợ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_HetDuNo: return "M_ResponseMessage_KUOCVM_HetDuNo";//Hết dư nợ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ThuocKhachHang: return "M_ResponseMessage_KUOCVM_ThuocKhachHang";//Thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_KhongThuocKhachHang: return "M_ResponseMessage_KUOCVM_KhongThuocKhachHang";//Không Thuộc khách hàng
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_SoTienGiaiNganKhongHopLe: return "M_ResponseMessage_KUOCVM_SoTienGiaiNganKhongHopLe";//Số tiền giải ngân không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_SoTienGiaiNganHopLe: return "M_ResponseMessage_KUOCVM_SoTienGiaiNganHopLe";//Số tiền giải ngân hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_VuotQuaHanMucChoPhep: return "M_ResponseMessage_KUOCVM_VuotQuaHanMucChoPhep";//Vượt quá hạn mức cho phép
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ChuaVuotQuaHanMucChoPhep: return "M_ResponseMessage_KUOCVM_ChuaVuotQuaHanMucChoPhep";//Vượt quá hạn mức cho phép
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_SoTienGocNhoHonSoTienGiaiNgan: return "M_ResponseMessage_KUOCVM_SoTienGocNhoHonSoTienGiaiNgan";//Số tiền gốc nhỏ hơn số tiền giải ngân
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_SoTienGocBangSoTienGiaiNgan: return "M_ResponseMessage_KUOCVM_SoTienGocBangSoTienGiaiNgan";//Số tiền gốc bằng số tiền giải ngân
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_SoTienGocLonHonSoTienGiaiNgan: return "M_ResponseMessage_KUOCVM_SoTienGocLonHonSoTienGiaiNgan";//Số tiền gốc lơn hơn số tiền giải ngân
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_TonTaiKWQuaHan: return "M_ResponseMessage_KUOCVM_TonTaiKWQuaHan";//Tồn tại khế ước quá hạn
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_KhongTonTaiKWQuaHan: return "M_ResponseMessage_KUOCVM_KhongTonTaiKWQuaHan";//Không Tồn tại khế ước quá hạn
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ChuaDamBaoTyLeTraNo: return "M_ResponseMessage_KUOCVM_ChuaDamBaoTyLeTraNo";//Chưa đảm bảo tỷ lệ trả nợ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_DamBaoTyLeTraNo: return "M_ResponseMessage_KUOCVM_DamBaoTyLeTraNo";//đảm bảo tỷ lệ trả nợ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NgayBDTraGocViPhamQuyDinh: return "M_ResponseMessage_KUOCVM_NgayBDTraGocViPhamQuyDinh";//Ngày bắt đầu trả gốc vi phạm quy định
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NgayBDTraGocKhongViPhamQuyDinh: return "M_ResponseMessage_KUOCVM_NgayBDTraGocKhongViPhamQuyDinh";//Ngày bắt đầu trả gốc không vi phạm quy định
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NgayBDTraLaiViPhamQuyDinh: return "M_ResponseMessage_KUOCVM_NgayBDTraLaiViPhamQuyDinh";//Ngày bắt đầu trả lãi vi phạm quy định
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NgayBDTraLaiKhongViPhamQuyDinh: return "M_ResponseMessage_KUOCVM_NgayBDTraLaiKhongViPhamQuyDinh";//Ngày bắt đầu trả lãi không vi phạm quy định
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_DKyDGiaLSuatHopLe: return "M_ResponseMessage_KUOCVM_DKyDGiaLSuatHopLe";//Định kỳ đánh giá lại lãi suất hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_DKyDGiaLSuatKhongHopLe: return "M_ResponseMessage_KUOCVM_DKyDGiaLSuatKhongHopLe";//Định kỳ đánh giá lại lãi suất không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ChuaDuocGiaiNgan: return "M_ResponseMessage_KUOCVM_ChuaDuocGiaiNgan";//Khế ước chưa được giải ngân
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_DaDuocGiaiNgan: return "M_ResponseMessage_KUOCVM_DaDuocGiaiNgan";//Khế ước đã được giải ngân
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NhomNo1: return "M_ResponseMessage_KUOCVM_NhomNo1";//Nhóm nợ 1
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NhomNo2: return "M_ResponseMessage_KUOCVM_NhomNo2";//Nhóm nợ 2
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NhomNo3: return "M_ResponseMessage_KUOCVM_NhomNo3";//Nhóm nợ 3
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NhomNo4: return "M_ResponseMessage_KUOCVM_NhomNo4";//Nhóm nợ 4
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_NhomNo5: return "M_ResponseMessage_KUOCVM_NhomNo5";//Nhóm nợ 5
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_KhongThuocNhomNo5: return "M_ResponseMessage_KUOCVM_KhongThuocNhomNo5";//Khong thuoc nhóm nợ 5
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_DaTatToan: return "M_ResponseMessage_KUOCVM_DaTatToan";//Đã tất toán
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ChuaTatToan: return "M_ResponseMessage_KUOCVM_ChuaTatToan";//Chưa tất toán
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_CuaKhachHangLienQuanChuaTatToan: return "M_ResponseMessage_KUOCVM_CuaKhachHangLienQuanChuaTatToan"; //KW của khách hàng liên quan Chưa tất toán
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_CuaKhachHangLienQuanDaTatToan: return "M_ResponseMessage_KUOCVM_CuaKhachHangLienQuanDaTatToan"; //KW của khách hàng liên quan đã tất toán
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_QuaHan: return "M_ResponseMessage_KUOCVM_QuaHan"; //KW quá hạn
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_KhongQuaHan: return "M_ResponseMessage_KUOCVM_KhongQuaHan"; //KW không quá hạn
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ThoiGianVayKhongHopLe: return "M_ResponseMessage_KUOCVM_ThoiGianVayKhongHopLe"; //KW thời gian vay không hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_ThoiGianVayHopLe: return "M_ResponseMessage_KUOCVM_ThoiGianVayHopLe"; //KW thời gian vay hợp lệ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_SoTienTKQDKhongDu: return "M_ResponseMessage_KUOCVM_SoTienTKQDKhongDu"; //KW số tiền TKQD không đủ
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_VayHaiKheUocCungMotSanPham: return "M_ResponseMessage_KUOCVM_VayHaiKheUocCungMotSanPham"; //KW vay hai khế ước cùng một sản phẩm
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_DaDuocPhanBoTrongThang: return "M_ResponseMessage_KUOCVM_DaDuocPhanBoTrongThang"; //Khế ước đã được phân bổ trong tháng
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_KyThuKhongHopLe: return "M_ResponseMessage_KUOCVM_KyThuKhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_KUOCVM_VayHaiKheUocCungMotKyHan: return "M_ResponseMessage_KUOCVM_VayHaiKheUocCungMotKyHan"; //KW vay hai khế ước cùng một ky han

                //Số giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SoGiaoDich_TonTai: return "M_ResponseMessage_SoGiaoDich_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_SoGiaoDich_KhongTonTai: return "M_ResponseMessage_SoGiaoDich_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_SoGiaoDich_KhongDuocPhepHoatDong: return "M_ResponseMessage_SoGiaoDich_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_SoGiaoDich_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_SoGiaoDich_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SoGiaoDich_DuocPhepGiaoDich: return "M_ResponseMessage_SoGiaoDich_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_SoGiaoDich_TaoKhongThanhCong: return "M_ResponseMessage_SoGiaoDich_TaoKhongThanhCong";//tạo không thành công

                //Giao dịch 
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongTonTai: return "M_ResponseMessage_GIAODICH_KhongTonTai";//Không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongThuocKiemSoatCuaPhongGD: return "M_ResponseMessage_GIAODICH_KhongThuocKiemSoatCuaPhongGD";//Giao dịch không thuộc kiểm soát của phòng giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongDuocPhepHoatDong: return "M_ResponseMessage_GIAODICH_KhongDuocPhepHoatDong";//tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_GIAODICH_ChuaDuocPhepGiaoDich";//tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_DuocPhepGiaoDich: return "M_ResponseMessage_GIAODICH_DuocPhepGiaoDich";//tồn tại, đang hoạt động, và được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_TonTai: return "M_ResponseMessage_GIAODICH_TonTai";//tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_ThongTinDuocPhepSua: return "M_ResponseMessage_GIAODICH_ThongTinDuocPhepSua";//Giao dịch bị sửa những thông tin giao dịch được phép sửa
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_ThongTinKhongDuocPhepSua: return "M_ResponseMessage_GIAODICH_ThongTinKhongDuocPhepSua";//Giao dịch bị sửa những thông tin không được phép sửa
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_ThemThanhCong: return "M_ResponseMessage_GIAODICH_ThemThanhCong";//Thêm giao dịch thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_SuaThanhCong: return "M_ResponseMessage_GIAODICH_SuaThanhCong";//Sửa giao dịch thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_XoaThanhCong: return "M_ResponseMessage_GIAODICH_XoaThanhCong";//Xóa giao dịch thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_DuyetThanhCong: return "M_ResponseMessage_GIAODICH_DuyetThanhCong";//Duyệt giao dịch thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_ThoaiDuyetThanhCong: return "M_ResponseMessage_GIAODICH_ThoaiDuyetThanhCong";//Thoái duyệt giao dịch thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_TuChoiDuyetThanhCong: return "M_ResponseMessage_GIAODICH_TuChoiDuyetThanhCong";//Từ chối duyệt giao dịch thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_ThemKhongThanhCong: return "M_ResponseMessage_GIAODICH_ThemKhongThanhCong";//Thêm giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_SuaKhongThanhCong: return "M_ResponseMessage_GIAODICH_SuaKhongThanhCong";//Sửa giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_XoaKhongThanhCong: return "M_ResponseMessage_GIAODICH_XoaKhongThanhCong";//Xóa giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_DuyetKhongThanhCong: return "M_ResponseMessage_GIAODICH_DuyetKhongThanhCong";//Duyệt giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_ThoaiDuyetKhongThanhCong: return "M_ResponseMessage_GIAODICH_ThoaiDuyetKhongThanhCong";//Thoái duyệt giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_TuChoiDuyetKhongThanhCong: return "M_ResponseMessage_GIAODICH_TuChoiDuyetKhongThanhCong";//Từ chối duyệt giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_TaoGiaoDichKhongThanhCong: return "M_ResponseMessage_GIAODICH_TaoGiaoDichKhongThanhCong"; //tạo giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_CapNhatThongTinGiaoDichKhongThanhCong: return "M_ResponseMessage_GIAODICH_CapNhatThongTinGiaoDichKhongThanhCong"; //cập nhât giao dịch không thành công
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongDuocPhepSua: return "M_ResponseMessage_GIAODICH_KhongDuocPhepSua"; //không được phép sửa
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongDuocPhepXoa: return "M_ResponseMessage_GIAODICH_KhongDuocPhepXoa"; //không được phép xóa
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongDuocPhepDuyet: return "M_ResponseMessage_GIAODICH_KhongDuocPhepDuyet"; //không được phép duyệt
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongDuocPhepThoaiDuyet: return "M_ResponseMessage_GIAODICH_KhongDuocPhepThoaiDuyet"; //không được phép thoái duyệt
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_KhongDuocPhepTuChoiDuyet: return "M_ResponseMessage_GIAODICH_KhongDuocPhepTuChoiDuyet"; //không được phép từ chối duyệt                    
                case NghiepVuResponseMessage.M_ResponseMessage_GIAODICH_TonTaiGiaoDichChuaDuocDuyet: return "M_ResponseMessage_GIAODICH_TonTaiGiaoDichChuaDuocDuyet"; //tồn tại giao dịch chưa được duyệt                    

                //Nghiệp vụ kế toán
                case NghiepVuResponseMessage.M_ResponseMessage_KeToan_ABC: return "M_ResponseMessage_KeToan_ABC";
                case NghiepVuResponseMessage.M_ResponseMessage_DinhKhoan_KhongDuocGhiNoVaCo: return "M_ResponseMessage_DinhKhoan_KhongDuocGhiNoVaCo";
                case NghiepVuResponseMessage.M_ResponseMessage_DinhKhoan_ChuaCoGhiNoHoacCo: return "M_ResponseMessage_DinhKhoan_ChuaCoGhiNoHoacCo";
                case NghiepVuResponseMessage.M_ResponseMessage_NhomDinhKhoan_KhongCoGhiNoHoacCo: return "M_ResponseMessage_NhomDinhKhoan_KhongCoGhiNoHoacCo";
                case NghiepVuResponseMessage.M_ResponseMessage_NhomDinhKhoan_ChiCoGhiNo: return "M_ResponseMessage_NhomDinhKhoan_ChiCoGhiNo";
                case NghiepVuResponseMessage.M_ResponseMessage_NhomDinhKhoan_ChiCoGhiCo: return "M_ResponseMessage_NhomDinhKhoan_ChiCoGhiCo";
                case NghiepVuResponseMessage.M_ResponseMessage_NhomDinhKhoan_NhieuNoNhieuCo: return "M_ResponseMessage_NhomDinhKhoan_NhieuNoNhieuCo";
                case NghiepVuResponseMessage.M_ResponseMessage_NhomDinhKhoan_TongNoKhongBangTongCo: return "M_ResponseMessage_NhomDinhKhoan_TongNoKhongBangTongCo";
                case NghiepVuResponseMessage.M_ResponseMessage_PhieuKeToan_KhongCoTaiKhoanTienMat: return "M_ResponseMessage_PhieuKeToan_KhongCoTaiKhoanTienMat";
                case NghiepVuResponseMessage.M_ResponseMessage_PhieuKeToan_KhongDuocGhiCoTienMat: return "M_ResponseMessage_PhieuKeToan_KhongDuocGhiCoTienMat";
                case NghiepVuResponseMessage.M_ResponseMessage_PhieuKeToan_KhongDuocGhiNoTienMat: return "M_ResponseMessage_PhieuKeToan_KhongDuocGhiNoTienMat";
                case NghiepVuResponseMessage.M_ResponseMessage_PhieuKeToan_KhongDuocTonTaiTienMat: return "M_ResponseMessage_PhieuKeToan_KhongDuocTonTaiTienMat";

                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_ThemKhongThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_ThemKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_ThemThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_ThemThanhCong";

                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_SuaKhongThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_SuaKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_SuaThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_SuaThanhCong";

                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_XoaKhongThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_XoaKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_XoaThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_XoaThanhCong";

                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_DuyetKhongThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_DuyetKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_DuyetThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_DuyetThanhCong";

                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_ThoaiDuyetKhongThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_ThoaiDuyetKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_ThoaiDuyetThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_ThoaiDuyetThanhCong";

                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_TuChoiDuyetKhongThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_TuChoiDuyetKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_PhanLoaiTaiKhoan_TuChoiDuyetThanhCong: return "M_ResponseMessage_PhanLoaiTaiKhoan_TuChoiDuyetThanhCong";

                case NghiepVuResponseMessage.M_ResponseMessage_KyHan_KhongHopLe: return "M_ResponseMessage_KyHan_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_KyHan_HopLe: return "M_ResponseMessage_KyHan_HopLe";

                case NghiepVuResponseMessage.M_ResponseMessage_NgayDaoHan_KhongHopLe: return "M_ResponseMessage_NgayDaoHan_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_NgayDaoHan_HopLe: return "M_ResponseMessage_NgayDaoHan_HopLe";

                case NghiepVuResponseMessage.M_ResponseMessage_KyHanDaoHan_KhongHopLe: return "M_ResponseMessage_KyHanDaoHan_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_KyHanDaoHan_HopLe: return "M_ResponseMessage_KyHanDaoHan_HopLe";

                //Nhân sự 
                case NghiepVuResponseMessage.M_ResponseMessage_HoSoNhanSu_KhongTonTai: return "M_ResponseMessage_HoSoNhanSu_KhongTonTai"; //Hồ sơ không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_HoSoNhanSu_KhongThuocPhongGD: return "M_ResponseMessage_HoSoNhanSu_KhongThuocPhongGD"; //Hồ sơ không thuộc kiểm soát của phòng
                case NghiepVuResponseMessage.M_ResponseMessage_HoSoNhanSu_KhongTaoDuoc: return "M_ResponseMessage_HoSoNhanSu_KhongTaoDuoc"; //Không tạo được mã hồ sơ
               
                case NghiepVuResponseMessage.M_ResponseMessage_HopDong_KhongTaoDuocMa: return "M_ResponseMessage_HopDong_KhongTaoDuocMa"; //Không tạo được số hợp đồng lao động
                case NghiepVuResponseMessage.M_ResponseMessage_HopDong_KhongTonTai: return "M_ResponseMessage_HopDong_KhongTonTai"; //Hợp đồng không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_HopDong_KhongThuocPhongGD: return "M_ResponseMessage_HopDong_KhongThuocPhongGD"; //Hợp đồng không thuộc kiểm soát của phòng
                
                case NghiepVuResponseMessage.M_ResponseMessage_ThuyenChuyenCongTac_KhongTaoDuocMa: return "M_ResponseMessage_ThuyenChuyenCongTac_KhongTaoDuocMa"; //Không tạo được số mã thuyên chuyển công tác
                case NghiepVuResponseMessage.M_ResponseMessage_ThuyenChuyenCongTac_KhongTonTai: return "M_ResponseMessage_ThuyenChuyenCongTac_KhongTonTai"; //Thuyên chuyển công tác không tồn tại     
                case NghiepVuResponseMessage.M_ResponseMessage_ThuyenChuyenCongTac_KhongThuocPhongGD: return "M_ResponseMessage_ThuyenChuyenCongTac_KhongThuocPhongGD"; //Thuyên chuyển công tác không kiểm soát của phòng
                
                case NghiepVuResponseMessage.M_ResponseMessage_ThoiViec_KhongTaoDuocMa: return "M_ResponseMessage_ThoiViec_KhongTaoDuocMa"; //Không tạo được số mã thôi việc
                case NghiepVuResponseMessage.M_ResponseMessage_ThoiViec_KhongTonTai: return "M_ResponseMessage_ThoiViec_KhongTonTai"; //Thôi việc không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_ThoiViec_KhongThuocPhongGD: return "M_ResponseMessage_ThoiViec_KhongThuocPhongGD"; //Thôi việc không kiểm soát của phòng

                case NghiepVuResponseMessage.M_ResponseMessage_ThongTinLuong_KhongTonTai: return "M_ResponseMessage_ThongTinLuong_KhongTonTai"; //Thông tin lương không tồn tại
                case NghiepVuResponseMessage.M_ResponseMessage_ThongTinLuong_KhongThuocPhongGD: return "M_ResponseMessage_ThongTinLuong_KhongThuocPhongGD"; //Thông tin lương không kiểm soát của phòng

                    //Ty gia
                case NghiepVuResponseMessage.M_ResponseMessage_TyGia_KhongTonTai: return "M_ResponseMessage_TyGia_KhongTonTai"; //Không tồn tại 
                case NghiepVuResponseMessage.M_ResponseMessage_TyGia_KhongDuocPhepHoatDong: return "M_ResponseMessage_TyGia_KhongDuocPhepHoatDong"; //tồn tại nhưng không được phép hoạt động
                case NghiepVuResponseMessage.M_ResponseMessage_TyGia_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_TyGia_ChuaDuocPhepGiaoDich"; //tồn tại, đang hoạt động, nhưng chưa được phép giao dịch
                case NghiepVuResponseMessage.M_ResponseMessage_TyGia_DuocPhepGiaoDich: return "M_ResponseMessage_TyGia_DuocPhepGiaoDich"; //tồn tại, đang hoạt động, và được phép giao dịch

                    //Giao dich doi tuong
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuongGDich_KhongTaoDuocDoiTuong: return "M_ResponseMessage_DoiTuongGDich_KhongTaoDuocDoiTuong";
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuongGDich_KhongTaoDuocMaDoiTuong: return "M_ResponseMessage_DoiTuongGDich_KhongTaoDuocMaDoiTuong";
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuongGDich_ThemDoiTuongKhongThanhCong: return "M_ResponseMessage_DoiTuongGDich_ThemDoiTuongKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuongGDich_DaTonTaiMaGoiNho: return "M_ResponseMessage_DoiTuongGDich_DaTonTaiMaGoiNho";
                    //Giao dich doi tuong
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuong_TinhChatSoDuKhongHopLe: return "M_ResponseMessage_DoiTuong_TinhChatSoDuKhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuong_KhongTaoDuocMaDoiTuong: return "M_ResponseMessage_DoiTuong_KhongTaoDuocMaDoiTuong";
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuong_ThemDoiTuongKhongThanhCong: return "M_ResponseMessage_DoiTuong_ThemDoiTuongKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_DoiTuong_TongSoDuKhongDung: return "M_ResponseMessage_DoiTuong_TongSoDuKhongDung";

                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_HanMucPheDuyetLonHonHMPheDuyetCapTong: return "M_ResponseMessage_HanMuc_HanMucPheDuyetLonHonHMPheDuyetCapTong";
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_ChuaNhapTaiSanDamBao: return "M_ResponseMessage_HanMuc_ChuaNhapTaiSanDamBao";
                case NghiepVuResponseMessage.M_ResponseMessage_HanMuc_TongTSDBToiDaLonHonHMCoTSDB: return "M_ResponseMessage_HanMuc_TongTSDBToiDaLonHonHMCoTSDB";

                    //Vong vay von
                case NghiepVuResponseMessage.M_ResponseMessage_VongVay_KhongHopLe : return "M_ResponseMessage_VongVay_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_VongVay_KhongTonTai : return "M_ResponseMessage_VongVay_KhongTonTai";
                case NghiepVuResponseMessage.M_ResponseMessage_VongVay_KhongDuocPhepHoatDong : return "M_ResponseMessage_VongVay_KhongDuocPhepHoatDong";
                case NghiepVuResponseMessage.M_ResponseMessage_VongVay_ChuaDuocPhepGiaoDich : return "M_ResponseMessage_VongVay_ChuaDuocPhepGiaoDich";

                    //Don xin vay
                case NghiepVuResponseMessage.M_ResponseMessage_DonVayVon_KhongHopLe : return "M_ResponseMessage_DonVayVon_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_DonVayVon_KhongTonTai: return "M_ResponseMessage_DonVayVon_KhongTonTai";
                case NghiepVuResponseMessage.M_ResponseMessage_DonVayVon_KhongDuocPhepHoatDong: return "M_ResponseMessage_DonVayVon_KhongDuocPhepHoatDong";
                case NghiepVuResponseMessage.M_ResponseMessage_DonVayVon_ChuaDuocPhepGiaoDich: return "M_ResponseMessage_DonVayVon_ChuaDuocPhepGiaoDich";
                case NghiepVuResponseMessage.M_ResponseMessage_DonVayVon_VuotHanMucGocVay: return "M_ResponseMessage_DonVayVon_VuotHanMucGocVay";
                case NghiepVuResponseMessage.M_ResponseMessage_DonVayVon_VuotHanMucThoiHanVay: return "M_ResponseMessage_DonVayVon_VuotHanMucThoiHanVay";
                case NghiepVuResponseMessage.M_ResponseMessage_Nhom_SoLuongKhachHangNhoHonQuyDinh: return "M_ResponseMessage_Nhom_SoLuongKhachHangNhoHonQuyDinh";
                case NghiepVuResponseMessage.M_ResponseMessage_Nhom_SoLuongKhachHangLonHonQuyDinh: return "M_ResponseMessage_Nhom_SoLuongKhachHangLonHonQuyDinh";

                //Khe uoc
                case NghiepVuResponseMessage.M_ResponseMessage_KheUoc_ThemThanhCong: return "M_ResponseMessage_KheUoc_ThemThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KheUoc_ThemKhongThanhCong: return "M_ResponseMessage_KheUoc_ThemKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KheUoc_DuyetThanhCong: return "M_ResponseMessage_KheUoc_DuyetThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KheUoc_DuyetKhongThanhCong: return "M_ResponseMessage_KheUoc_DuyetKhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KheUoc_TonTaiKiemSoatRuiRo: return "M_ResponseMessage_KheUoc_TonTaiKiemSoatRuiRo";

                // Kiểm soát rủi ro
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_KheUocDaGiaiNgan: return "M_ResponseMessage_KiemSoatRuiRo_KheUocDaGiaiNgan";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_ThemMaXacNhan_KhongThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_ThemMaXacNhan_KhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_ThemMaXacNhan_ThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_ThemMaXacNhan_ThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_CapNhatMaXacNhan_KhongThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_CapNhatMaXacNhan_KhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_CapNhatMaXacNhan_ThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_CapNhatMaXacNhan_ThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_XoaMaXacNhan_KhongThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_XoaMaXacNhan_KhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_XoaMaXacNhan_ThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_XoaMaXacNhan_ThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_TaoMaXacNhan_KhongThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_TaoMaXacNhan_KhongThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_KiemSoatRuiRo_TaoMaXacNhan_ThanhCong: return "M_ResponseMessage_KiemSoatRuiRo_TaoMaXacNhan_ThanhCong";

                case NghiepVuResponseMessage.M_ResponseMessage_Email_QueueKhongTonTai: return "M_ResponseMessage_Email_QueueKhongTonTai";
                case NghiepVuResponseMessage.M_ResponseMessage_Email_QueueThanhCong: return "M_ResponseMessage_Email_QueueThanhCong";
                case NghiepVuResponseMessage.M_ResponseMessage_Email_QueueHoanThanh: return "M_ResponseMessage_Email_QueueHoanThanh";

                //Lãi suất trần sàn
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatSanKKH_KhongHopLe: return "M_ResponseMessage_LaiSuatSanKKH_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatTranKKH_KhongHopLe: return "M_ResponseMessage_LaiSuatTranKKH_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatSanCKH_KhongHopLe: return "M_ResponseMessage_LaiSuatSanCKH_KhongHopLe";
                case NghiepVuResponseMessage.M_ResponseMessage_LaiSuatTranCKH_KhongHopLe: return "M_ResponseMessage_LaiSuatTranCKH_KhongHopLe";
                    
                default: return "";
            }
        }

        public enum LoaiTaiKhoan
        {
            NOI_BANG,
            NGOAI_BANG
        }

        public static string layGiaTri(this LoaiTaiKhoan loaiTaiKhoan)
        {
            switch (loaiTaiKhoan)
            {
                case LoaiTaiKhoan.NOI_BANG: return "NOI_BANG";
                case LoaiTaiKhoan.NGOAI_BANG: return "NGOAI_BANG";
                default: return "";
            }
        }

        public enum LoaiTaiKhoanTheoDoiTuong
        {
            KHACH_HANG,
            NOI_BO
        }

        public static string layGiaTri(this LoaiTaiKhoanTheoDoiTuong loaiTaiKhoan)
        {
            switch (loaiTaiKhoan)
            {
                case LoaiTaiKhoanTheoDoiTuong.KHACH_HANG: return "KHACH_HANG";
                case LoaiTaiKhoanTheoDoiTuong.NOI_BO: return "NOI_BO";
                default: return "";
            }
        }

        public enum LoaiTaiKhoanMucDich
        {
            THU_NHAP,
            CHI_PHI,
            KHAC
        }

        public static string layGiaTri(this LoaiTaiKhoanMucDich loaiTaiKhoan)
        {
            switch (loaiTaiKhoan)
            {
                case LoaiTaiKhoanMucDich.THU_NHAP: return "THU_NHAP";
                case LoaiTaiKhoanMucDich.CHI_PHI: return "CHI_PHI";
                case LoaiTaiKhoanMucDich.KHAC: return "KHAC";
                default: return "";
            }
        }

        #region Đơn vị sử dụng
        public enum DonViSuDung
        {
            M7MFI,
            BINHKHANH,
            BANTAYVANG,
            BENTRE,
            HOCVIENNGANHANG,
            PHUTHO,
            BIDV,
            QUANGBINH,
            MOM,
            BIDV_BLF,
            NGVGROUP
        }
        public static string layGiaTri(this DonViSuDung donViSuDung)
        {
            switch (donViSuDung)
            {
                case DonViSuDung.M7MFI: return "M7MFI";
                case DonViSuDung.BINHKHANH: return "BINHKHANH";
                case DonViSuDung.BANTAYVANG: return "BANTAYVANG";
                case DonViSuDung.BENTRE: return "BENTRE";
                case DonViSuDung.HOCVIENNGANHANG: return "HOCVIENNGANHANG";
                case DonViSuDung.PHUTHO: return "PHUTHO";
                case DonViSuDung.BIDV: return "BIDV";
                case DonViSuDung.QUANGBINH: return "QUANGBINH";
                case DonViSuDung.MOM: return "MOM";
                case DonViSuDung.BIDV_BLF: return "BIDV_BLF";
                case DonViSuDung.NGVGROUP: return "NGVGROUP";
                default: return "NGVGROUP";
            }
        }
        public static DonViSuDung layDonViSuDung(this string donViSuDung)
        {
            switch (donViSuDung)
            {
                case "M7MFI": return DonViSuDung.M7MFI;
                case "BINHKHANH": return DonViSuDung.BINHKHANH;
                case "BANTAYVANG": return DonViSuDung.BANTAYVANG;
                case "BENTRE": return DonViSuDung.BENTRE;
                case "HOCVIENNGANHANG": return DonViSuDung.HOCVIENNGANHANG;
                case "PHUTHO": return DonViSuDung.PHUTHO;
                case "BIDV": return DonViSuDung.BIDV;
                case "QUANGBINH": return DonViSuDung.QUANGBINH;
                case "MOM": return DonViSuDung.MOM;
                case "BIDV_BLF": return DonViSuDung.BIDV_BLF;
                case "NGVGROUP": return DonViSuDung.NGVGROUP;
                default: return DonViSuDung.NGVGROUP;
            }
        }
        #endregion


        public enum TinhNangWebIcon
        {
            View,
            Add,
            Modify,
            Delete,
            Approve,
            Cancel,
            Refuse,
            Clone,
            Print,
            Export,
            Import,
            Hold,
            Save,
            Search,
            Reload,
            Help,
            Close,
            Copy,
            Cut,
            Paste,
            Filter,
            Preview,
            Synch,
            Connect,
            Disconnect,
            Popup,
            Collect,
            Make,
            Generate,
            Caculate,
            Execute,
            Process,
            Config,
            Download,
            Upload,
            Clean,
            Backup,
            Restore,
            CashStmt,
            FullControl
        }

        public static string layGiaTri(this TinhNangWebIcon tinhNang)
        {
            switch (tinhNang)
            {
                case TinhNangWebIcon.View: return "View";
                case TinhNangWebIcon.Add: return "Add";
                case TinhNangWebIcon.Modify: return "Modify";
                case TinhNangWebIcon.Delete: return "Delete";
                case TinhNangWebIcon.Approve: return "Approve";
                case TinhNangWebIcon.Cancel: return "Cancel";
                case TinhNangWebIcon.Refuse: return "Refuse";
                case TinhNangWebIcon.Clone: return "Clone";
                case TinhNangWebIcon.Print: return "Print";
                case TinhNangWebIcon.Export: return "Export";
                case TinhNangWebIcon.Import: return "Import";
                case TinhNangWebIcon.Hold: return "Hold";
                case TinhNangWebIcon.Save: return "Save";
                case TinhNangWebIcon.Search: return "Search";
                case TinhNangWebIcon.Reload: return "Reload";
                case TinhNangWebIcon.Help: return "Help";
                case TinhNangWebIcon.Close: return "Close";
                case TinhNangWebIcon.Copy: return "Copy";
                case TinhNangWebIcon.Cut: return "Cut";
                case TinhNangWebIcon.Paste: return "Paste";
                case TinhNangWebIcon.Filter: return "Filter";
                case TinhNangWebIcon.Preview: return "Preview";
                case TinhNangWebIcon.Synch: return "Synch";
                case TinhNangWebIcon.Connect: return "Connect";
                case TinhNangWebIcon.Disconnect: return "Disconnect";
                case TinhNangWebIcon.Popup: return "Popup";
                case TinhNangWebIcon.Collect: return "Collect";
                case TinhNangWebIcon.Make: return "Make";
                case TinhNangWebIcon.Generate: return "Generate";
                case TinhNangWebIcon.Caculate: return "Caculate";
                case TinhNangWebIcon.Execute: return "Execute";
                case TinhNangWebIcon.Process: return "Process";
                case TinhNangWebIcon.Config: return "Config";
                case TinhNangWebIcon.Download: return "Download";
                case TinhNangWebIcon.Upload: return "Upload";
                case TinhNangWebIcon.Clean: return "Clean";
                case TinhNangWebIcon.Backup: return "Backup";
                case TinhNangWebIcon.Restore: return "Restore";
                case TinhNangWebIcon.CashStmt: return "CashStmt";
                case TinhNangWebIcon.FullControl: return "FullControl";
                default: return "";
            }
        }

        public static string layGiaTriIcon(string tinhNang)
        {
            switch (tinhNang)
            {
                case "View": return "fa-file-text-o";
                case "Add": return "fa-file-o";
                case "Modify": return "fa-edit";
                case "Delete": return "fa-trash-o";
                case "Approve": return "fa-check-square-o";
                case "Cancel": return "fa-ban";
                case "Refuse": return "fa-undo";
                case "Clone": return "fa-files-o";
                case "Print": return "fa-print";
                case "Export": return "fa-file-o";
                case "Import": return "fa-file-o";
                case "Hold": return "fa-file-o";
                case "Save": return "fa-floppy-o";
                case "Search": return "fa-search";
                case "Reload": return "fa-refresh";
                case "Help": return "fa-question-circle";
                case "Close": return "fa-question-circle";
                case "Copy": return "fa-files-o";
                case "Cut": return "fa-cut";
                case "Paste": return "fa-paste";
                case "Filter": return "fa-filter";
                case "Preview": return "fa-file-o";
                case "Synch": return "fa-file-o";
                case "Connect": return "fa-file-o";
                case "Disconnect": return "fa-file-o";
                case "Popup": return "fa-file-o";
                case "Collect": return "fa-file-o";
                case "Make": return "fa-file-o";
                case "Generate": return "fa-sitemap";
                case "Caculate": return "fa-keyboard-o";
                case "Execute": return "fa-tasks";
                case "Process": return "fa-spinner";
                case "Config": return "fa-cogs";
                case "Download": return "fa-download";
                case "Upload": return "fa-upload";
                case "Clean": return "fa-file-o";
                case "Backup": return "fa-file-o";
                case "Restore": return "fa-file-o";
                case "CashStmt": return "fa-file-o";
                case "FullControl": return "fa-file-o";
                default: return "";
            }
        }
    }

    public class KIEM_SOAT
    {
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _SO_GIAO_DICH;
        public string SO_GIAO_DICH
        {
            get { return _SO_GIAO_DICH; }
            set { _SO_GIAO_DICH = value; }
        }

        private string _MA_PHAN_HE;
        public string MA_PHAN_HE
        {
            get { return _MA_PHAN_HE; }
            set { _MA_PHAN_HE = value; }
        }

        private string _MA_LOAI_GD;
        public string MA_LOAI_GD
        {
            get { return _MA_LOAI_GD; }
            set { _MA_LOAI_GD = value; }
        }

        string _MA_TCHIEU;

        public string MA_TCHIEU
        {
            get { return _MA_TCHIEU; }
            set { _MA_TCHIEU = value; }
        }

        private DatabaseConstant.Action _action;
        public DatabaseConstant.Action action
        {
            get { return _action; }
            set { _action = value; }
        }

        private string _TTHAI_NVU;
        public string TTHAI_NVU
        {
            get { return _TTHAI_NVU; }
            set { _TTHAI_NVU = value; }
        }
    }

    public class TTHAI_LY_DO
    {
        private int _iD;

        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        private string _mA;

        public string MA
        {
            get { return _mA; }
            set { _mA = value; }
        }
        private string _tEN;

        public string TEN
        {
            get { return _tEN; }
            set { _tEN = value; }
        }
        private string _lY_DO;

        public string LY_DO
        {
            get { return _lY_DO; }
            set { _lY_DO = value; }
        }
    }

    public class CAP_PHE_DUYET
    {
        private int _iD;

        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        private int _iD_TCHIEU;

        public int ID_TCHIEU
        {
            get { return _iD_TCHIEU; }
            set { _iD_TCHIEU = value; }
        }
        private string _mA_TCHIEU;

        public string MA_TCHIEU
        {
            get { return _mA_TCHIEU; }
            set { _mA_TCHIEU = value; }
        }
        private string _mO_TA;

        public string MO_TA
        {
            get { return _mO_TA; }
            set { _mO_TA = value; }
        }
        private string _fUNCTION;

        public string FUNCTION
        {
            get { return _fUNCTION; }
            set { _fUNCTION = value; }
        }
        private string _mA_CAP_PHE_DUYET;

        public string MA_CAP_PHE_DUYET
        {
            get { return _mA_CAP_PHE_DUYET; }
            set { _mA_CAP_PHE_DUYET = value; }
        }
        private decimal _hAN_MUC_PHE_DUYET;

        public decimal HAN_MUC_PHE_DUYET
        {
            get { return _hAN_MUC_PHE_DUYET; }
            set { _hAN_MUC_PHE_DUYET = value; }
        }
        private string _lY_DO;

        public string LY_DO
        {
            get { return _lY_DO; }
            set { _lY_DO = value; }
        }
        private decimal _mUC_PHE_DUYET;

        public decimal MUC_PHE_DUYET
        {
            get { return _mUC_PHE_DUYET; }
            set { _mUC_PHE_DUYET = value; }
        }
        private string _tRANG_THAI;

        public string TRANG_THAI
        {
            get { return _tRANG_THAI; }
            set { _tRANG_THAI = value; }
        }
        private string _uSER_NAME;

        public string USER_NAME
        {
            get { return _uSER_NAME; }
            set { _uSER_NAME = value; }
        }
        private List<string> _dSACH_TTHAI;

        public List<string> DSACH_TTHAI
        {
            get { return _dSACH_TTHAI; }
            set { _dSACH_TTHAI = value; }
        }
        private bool _bResult;

        public bool BResult
        {
            get { return _bResult; }
            set { _bResult = value; }
        }
        private string _lOAI_TIEN;

        public string LOAI_TIEN
        {
            get { return _lOAI_TIEN; }
            set { _lOAI_TIEN = value; }
        }
        private string _aCTION;

        public string ACTION
        {
            get { return _aCTION; }
            set { _aCTION = value; }
        }
        
    }

    public class OBJ_INPUT
    {
        private int _iD;

        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        private string _mA;

        public string MA
        {
            get { return _mA; }
            set { _mA = value; }
        }
        private DatabaseConstant.Action _aCTION;

        public DatabaseConstant.Action ACTION
        {
            get { return _aCTION; }
            set { _aCTION = value; }
        }
        private DatabaseConstant.Function _fUNCTION;

        public DatabaseConstant.Function FUNCTION
        {
            get { return _fUNCTION; }
            set { _fUNCTION = value; }
        }
        private string _tTHAI_NVU;

        public string TTHAI_NVU
        {
            get { return _tTHAI_NVU; }
            set { _tTHAI_NVU = value; }
        }
    }

    public class ErrorDef
    {
        public const int SYSTEM_SUCESS = 0;
        public const int SYSTEM_ERROR = -1;
        public const int SYSTEM_ERROR_FUNCNAME_NOT_FOUND = -2;
        public const int SYSTEM_ERROR_NO_DATA_FOUND= -3;
        public const int USER_STATUS_INVALID=-1000001;
        public const int USER_PASS_MUST_CHANGE = -1000002;
        public const int USER_NOTACTIVE = -1000003;
        public const int USER_PARAMINVALID = -1000004;
        public const int USER_NOT_FOUND = -1000005;
        public const int USER_PASS_LESS_THAN_MIN = -1000006;
        public const int USER_PASS_CONFIRM_NOT_MATCHED = -1000007;
        public const int USER_PASS_WRONG = -1000008;

        public const int KHTV_CUM_TENCUMKHONGHOPLE = -2000001;
        public const int KHTV_CUM_TENTATKHONGHOPLE = -2000002;
        public const int KHTV_CUM_NGAYTLKHONGHOPLE = -2000003;
    }
}
