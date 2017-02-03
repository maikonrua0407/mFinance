using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Common
{
    /// <summary>
    /// @Truongnx on 20121010
    /// Lưu các thông tin global nghiệp vụ
    /// - Module nghiệp vụ
    /// - Chức năng nghiệp vụ
    /// - Quyền người dùng trong hệ thống
    /// - Trạng thái nghiệp vụ
    /// - Yêu cầu đổi mật khẩu
    /// - Loại đối tượng
    /// - Loại tài nguyên
    /// - Trạng thái bản ghi
    /// </summary>
    public static class BusinessConstant
    {
        public static string asposeExcelLic = "<?xml version=\"1.0\" encoding=\"utf-8\"?><License><Data><LicensedTo>RTU</LicensedTo><LicenseType>Developer Small Business</LicenseType><LicenseNote>Limited to 1 developer.</LicenseNote><OrderID>121119042331</OrderID><OEM>This is not a non-redistributable license</OEM><Products><Product>Aspose.Cells for .NET</Product></Products><EditionType>Enterprise</EditionType><SerialNumber>9624acd1-254f-4e42-b530-054366b90433</SerialNumber><SubscriptionExpiry>20131119</SubscriptionExpiry><LicenseVersion>2.2</LicenseVersion></Data><Signature>hPRRA9rfpG3FVornybxKSleJGY5iNzIXbmaPCH4v8DaT3cBXMI/dTcq45qBcsoaBPH8DFVhAPbeiXKZQQDmiDDFLDR/nyYPIafRozJzAC9lojbiebtqIX/ABjfls58z5gv2TV0pD6l+sdPFatbMUFey6O15fJmfuaNdqpPg+aXI=</Signature></License>";
        public static string asposeWordLic = "<?xml version=\"1.0\" encoding=\"utf-8\"?><License><Data><SerialNumber>aed83727-21cc-4a91-bea4-2607bf991c21</SerialNumber><EditionType>Enterprise</EditionType><Products><Product>Aspose.Total</Product></Products></Data><Signature>CxoBmxzcdRLLiQi1kzt5oSbz9GhuyHHOBgjTf5w/wJ1V+lzjBYi8o7PvqRwkdQo4tT4dk3PIJPbH9w5Lszei1SV/smkK8SCjR8kIWgLbOUFBvhD1Fn9KgDAQ8B11psxIWvepKidw8ZmDmbk9kdJbVBOkuAESXDdtDEDZMB/zL7Y=</Signature></License>";

        /// <summary>
        /// Module nghiệp vụ
        /// </summary>
        private enum ModuleNghiepVu
        {
            QuanTriHeThong,
            QuanTriKhachHang,
            TinDung
        }

        /// <summary>
        /// Chức năng nghiệp vụ
        /// </summary>
        private enum ChucNangNghiepVu
        {
            DM_TINH_THANH,
            DM_DON_VI
        }

        /// <summary>
        /// Quyền người dùng trong hệ thống
        /// </summary>
        public enum Quyen
        {
            THEM,
            SUA,
            XOA,
            DUYET,
            TU_CHOI_DUYET,
            THOAI_DUYET,
            HUY_DUYET,
            DEFAULT
        }

        public static string layGiaTri(this Quyen quyen)
        {
            switch (quyen)
            {
                case Quyen.THEM: return "THEM";
                case Quyen.SUA: return "SUA";
                case Quyen.XOA: return "XOA";
                case Quyen.DUYET: return "DUYET";
                case Quyen.TU_CHOI_DUYET: return "TU_CHOI_DUYET";
                case Quyen.THOAI_DUYET: return "THOAI_DUYET";
                default: return "DEFAULT";
            }
        }
        public static Quyen layQuyen(string quyen)
        {
            switch (quyen)
            {
                case "THEM": return Quyen.THEM;
                case "SUA": return Quyen.SUA;
                case "XOA": return Quyen.XOA;
                case "DUYET": return Quyen.DUYET;
                case "TU_CHOI_DUYET": return Quyen.TU_CHOI_DUYET;
                case "THOAI_DUYET": return Quyen.THOAI_DUYET;
                default: return Quyen.DEFAULT;
            }
        }

        /// <summary>
        /// Tính chất số dư
        /// </summary> 
        public enum TinhChatSoDu
        {
            NO, //Nợ
            CO, //Có
            LT, //Lưỡng tính
            DEFAULT //Mắc định
        }
        public static string layGiaTri(this TinhChatSoDu tinhChatSoDu)
        {
            switch (tinhChatSoDu)
            {
                case TinhChatSoDu.NO: return "NO";
                case TinhChatSoDu.CO: return "CO";
                case TinhChatSoDu.LT: return "LT";
                default: return "LT";
            }
        }

        /// <summary>
        /// Trạng thái đóng mở
        /// </summary> 
        public enum TrangThaiDongMo
        {
            DONG, //Đóng
            MO, //Mở
            DEFAULT //Mặc định
        }
        public static string layGiaTri(this TrangThaiDongMo trangThaiDongMo)
        {
            switch (trangThaiDongMo)
            {
                case TrangThaiDongMo.DONG: return "DONG";
                case TrangThaiDongMo.MO: return "MO";
                default: return "DONG";
            }
        }

        /// <summary>
        /// Loại tỷ giá
        /// </summary> 
        public enum LoaiTyGia
        {
            LIEN_NGAN_HANG, //Liên ngân hàng
            CONG_CHUNG, //Công chúng
            DEFAULT //Mặc định
        }
        public static string layGiaTri(this LoaiTyGia loaiTyGia)
        {
            switch (loaiTyGia)
            {
                case LoaiTyGia.LIEN_NGAN_HANG: return "LIEN_NGAN_HANG";
                case LoaiTyGia.CONG_CHUNG: return "CONG_CHUNG";
                default: return "TRUC_TIEP";
            }
        }

        /// <summary>
        /// Hình thức niêm yết
        /// </summary> 
        public enum HinhThucNiemYet
        {
            TRUC_TIEP, //Trực tiếp
            GIAN_TIEP, //Gián tiếp
            DEFAULT //Mặc định
        }
        public static string layGiaTri(this HinhThucNiemYet hinhThucNiemYet)
        {
            switch (hinhThucNiemYet)
            {
                case HinhThucNiemYet.TRUC_TIEP: return "TRUC_TIEP";
                case HinhThucNiemYet.GIAN_TIEP: return "GIAN_TIEP";
                default: return "TRUC_TIEP";
            }
        }


        /// <summary>
        /// Trạng thái nghiệp vụ trong hệ thống
        /// </summary> 
        public enum TrangThaiNghiepVu
        {
            LUU_TAM,
            CHO_DUYET,
            SUA_SAU_DUYET,
            DA_DUYET,
            TU_CHOI,
            THOAI_DUYET,
            LUU_TAM_SUA_SAU_DUYET,
            TU_CHOI_SUA_SAU_DUYET,
            TU_CHOI_CAP_TIN_DUNG,
            DEFAULT
        }

        public static string layGiaTri(this TrangThaiNghiepVu trangThaiNghiepVu)
        {
            switch (trangThaiNghiepVu)
            {
                case TrangThaiNghiepVu.LUU_TAM: return "LTA";
                case TrangThaiNghiepVu.CHO_DUYET: return "CDU";
                case TrangThaiNghiepVu.SUA_SAU_DUYET: return "SSD";
                case TrangThaiNghiepVu.DA_DUYET: return "DDU";
                case TrangThaiNghiepVu.TU_CHOI: return "TCD";
                case TrangThaiNghiepVu.THOAI_DUYET: return "THD";
                case TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET: return "LTA_SSD";
                case TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET: return "TCD_SSD";
                case TrangThaiNghiepVu.TU_CHOI_CAP_TIN_DUNG: return "TCD_CTD";
                default: return "DEFAULT";
            }
        }

        public static TrangThaiNghiepVu layTrangThaiNghiepVu(string trangThaiNghiepVu)
        {
            switch (trangThaiNghiepVu)
            {
                case "LTA": return TrangThaiNghiepVu.LUU_TAM;
                case "CDU": return TrangThaiNghiepVu.CHO_DUYET;
                case "SSD": return TrangThaiNghiepVu.SUA_SAU_DUYET;
                case "DDU": return TrangThaiNghiepVu.DA_DUYET;
                case "TCD": return TrangThaiNghiepVu.TU_CHOI;
                case "THD": return TrangThaiNghiepVu.THOAI_DUYET;
                case "LTA_SSD": return TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET;
                case "TCD_SSD": return TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET;
                case "TCD_CTD": return TrangThaiNghiepVu.TU_CHOI_CAP_TIN_DUNG;
                default: return TrangThaiNghiepVu.DEFAULT;
            }
        }
        /// <summary>
        /// Lấy ngôn ngữ hiển thị trạng thái nghiệp vụ
        /// </summary>
        /// <param name="trangthai">string TrangThaiNghiepVu</param>
        /// <returns>trạng thái nghiệp vụ</returns>
        public static string layNgonNguNghiepVu(string trangthai)
        {
            if (trangthai == layGiaTri(TrangThaiNghiepVu.LUU_TAM))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.LuuTam");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.CHO_DUYET))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.ChoDuyet");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.SUA_SAU_DUYET))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.SuaSauDuyet");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.DA_DUYET))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.DaDuyet");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.TU_CHOI))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.TuChoi");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.THOAI_DUYET))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.ThoaiDuyet");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.LuuTamSuaSauDuyet");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.TuChoiSuaSauDuyet");
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.TU_CHOI_CAP_TIN_DUNG))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.TuChoi");
            }

            return "";
        }

        public static string layMaNgonNguNghiepVu(string trangthai)
        {
            if (trangthai == layGiaTri(TrangThaiNghiepVu.LUU_TAM))
            {
                return "U.DungChung.TrangThaiNghiepVu.LuuTam";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.CHO_DUYET))
            {
                return "U.DungChung.TrangThaiNghiepVu.ChoDuyet";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.SUA_SAU_DUYET))
            {
                return "U.DungChung.TrangThaiNghiepVu.SuaSauDuyet";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.DA_DUYET))
            {
                return "U.DungChung.TrangThaiNghiepVu.DaDuyet";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.TU_CHOI))
            {
                return "U.DungChung.TrangThaiNghiepVu.TuChoi";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.THOAI_DUYET))
            {
                return "U.DungChung.TrangThaiNghiepVu.ThoaiDuyet";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET))
            {
                return "U.DungChung.TrangThaiNghiepVu.LuuTamSuaSauDuyet";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET))
            {
                return "U.DungChung.TrangThaiNghiepVu.TuChoiSuaSauDuyet";
            }
            else if (trangthai == layGiaTri(TrangThaiNghiepVu.TU_CHOI_CAP_TIN_DUNG))
            {
                return "U.DungChung.TrangThaiNghiepVu.TuChoi";
            }

            return "";
        }

        public enum TrangThaiCapTinDung
        {
            CDU,
            DDU,
            TCD,
            TCD_CTD,
            THD,
            DEFAULT
        }

        public static string layGiaTri(this TrangThaiCapTinDung item)
        {
            switch (item)
            {
                case TrangThaiCapTinDung.CDU: return "CDU";
                case TrangThaiCapTinDung.DDU: return "DDU";
                case TrangThaiCapTinDung.TCD: return "TCD";
                case TrangThaiCapTinDung.TCD_CTD: return "TCD_CTD";
                case TrangThaiCapTinDung.THD: return "THD";
                default: return "DEFAULT";
            }
        }

        public static TrangThaiCapTinDung layTrangThaiCapTinDung(string item)
        {
            switch (item)
            {
                case "CDU": return TrangThaiCapTinDung.CDU;
                case "DDU": return TrangThaiCapTinDung.DDU;
                case "TCD": return TrangThaiCapTinDung.TCD;
                case "TCD_CTD": return TrangThaiCapTinDung.TCD_CTD;
                case "THD": return TrangThaiCapTinDung.THD;
                default: return TrangThaiCapTinDung.DEFAULT;
            }
        }

        public static string layNgonNguCapTinDung(string item)
        {
            if (item == layGiaTri(TrangThaiCapTinDung.CDU))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.CDU");
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.DDU))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.DDU");
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.TCD))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.TCD");
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.TCD_CTD))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.TCD_CTD");
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.THD))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.THD");
            }

            return "";
        }

        public static string layMaNgonNguCapTinDung(string item)
        {
            if (item == layGiaTri(TrangThaiCapTinDung.CDU))
            {
                return "U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.CDU";
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.DDU))
            {
                return "U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.DDU";
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.TCD))
            {
                return "U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.TCD";
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.TCD_CTD))
            {
                return "U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.TCD_CTD";
            }
            else if (item == layGiaTri(TrangThaiCapTinDung.THD))
            {
                return "U.DMUC_GTRI.TRANG_THAI_CAP_TIN_DUNG.THD";
            }

            return "";
        }

        public enum TrangThaiNguoiDung
        {
            SU_DUNG,
            KHONG_SU_DUNG,
            TAM_KHOA,
            DEFAULT
        }

        public static string layGiaTri(this TrangThaiNguoiDung trangThaiNguoiDung)
        {
            switch (trangThaiNguoiDung)
            {
                case TrangThaiNguoiDung.SU_DUNG: return "SU_DUNG";
                case TrangThaiNguoiDung.KHONG_SU_DUNG: return "KHONG_SU_DUNG";
                case TrangThaiNguoiDung.TAM_KHOA: return "TAM_KHOA";
                default: return "DEFAULT";
            }
        }

        public static TrangThaiNguoiDung layTrangThaiNguoiDung(string trangThaiNguoiDung)
        {
            switch (trangThaiNguoiDung)
            {
                case "SU_DUNG": return TrangThaiNguoiDung.SU_DUNG;
                case "KHONG_SU_DUNG": return TrangThaiNguoiDung.KHONG_SU_DUNG;
                case "TAM_KHOA": return TrangThaiNguoiDung.TAM_KHOA;
                default: return TrangThaiNguoiDung.DEFAULT;
            }
        }

        public static string layNgonNguTrangThaiNguoiDung(string trangthai)
        {
            if (trangthai == layGiaTri(TrangThaiNguoiDung.SU_DUNG))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNguoiDung.SuDung");
            }
            else if (trangthai == layGiaTri(TrangThaiNguoiDung.KHONG_SU_DUNG))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNguoiDung.KhongSuDung");
            }
            else if (trangthai == layGiaTri(TrangThaiNguoiDung.TAM_KHOA))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNguoiDung.TamKhoa");
            }

            return "";
        }

        public static string layMaNgonNguTrangThaiNguoiDung(string trangthai)
        {
            if (trangthai == layGiaTri(TrangThaiNguoiDung.SU_DUNG))
            {
                return "U.DungChung.TrangThaiNguoiDung.SuDung";
            }
            else if (trangthai == layGiaTri(TrangThaiNguoiDung.KHONG_SU_DUNG))
            {
                return "U.DungChung.TrangThaiNguoiDung.KhongSuDung";
            }
            else if (trangthai == layGiaTri(TrangThaiNguoiDung.TAM_KHOA))
            {
                return "U.DungChung.TrangThaiNguoiDung.TamKhoa";
            }

            return "";
        }

        /// <summary>
        /// Lấy ngôn ngữ hiển thị trạng thái bản ghi
        /// </summary>
        /// <param name="trangthai">string TrangThaiBanGhi</param>
        /// <returns>trạng thái bản ghi</returns>
        public static string layNgonNguSuDung(string trangthai)
        {
            if (trangthai == layGiaTri(TrangThaiBanGhi.SU_DUNG))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiBanGhi.SuDung");
            }
            else if (trangthai == layGiaTri(TrangThaiBanGhi.KHONG_SU_DUNG))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.TrangThaiBanGhi.KhongSuDung");
            }

            return "";
        }

        public static string layMaNgonNguSuDung(string trangthai)
        {
            if (trangthai == layGiaTri(TrangThaiBanGhi.SU_DUNG))
            {
                return "U.DungChung.TrangThaiBanGhi.SuDung";
            }
            else if (trangthai == layGiaTri(TrangThaiBanGhi.KHONG_SU_DUNG))
            {
                return "U.DungChung.TrangThaiBanGhi.KhongSuDung";
            }

            return "";
        }

        /// <summary>
        /// Lấy ngôn ngữ hiển thị loại người dùng
        /// </summary>
        /// <param name="trangthai">string LoaiNguoiSuDung</param>
        /// <returns>loại người dùng</returns>
        public static string layNgonNguLoaiNguoiDung(string trangthai)
        {
            if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_SA))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.PhanLoaiNguoiDung.CAP_SA");
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_QTTW))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.PhanLoaiNguoiDung.CAP_QTTW");
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_NVTW))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.PhanLoaiNguoiDung.CAP_NVTW");
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_QTDV))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.PhanLoaiNguoiDung.CAP_QTDV");
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_NVDV))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.PhanLoaiNguoiDung.CAP_NVDV");
            }

            return "";
        }

        public static string layMaNgonNguLoaiNguoiDung(string trangthai)
        {
            if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_SA))
            {
                return "U.DungChung.PhanLoaiNguoiDung.CAP_SA";
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_QTTW))
            {
                return "U.DungChung.PhanLoaiNguoiDung.CAP_QTTW";
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_NVTW))
            {
                return "U.DungChung.PhanLoaiNguoiDung.CAP_NVTW";
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_QTDV))
            {
                return "U.DungChung.PhanLoaiNguoiDung.CAP_QTDV";
            }
            else if (trangthai == layGiaTri(LoaiNguoiSuDung.CAP_NVDV))
            {
                return "U.DungChung.PhanLoaiNguoiDung.CAP_NVDV";
            }

            return "";
        }

        /// <summary>
        /// Lấy ngôn ngữ hiển thị phương pháp tính lãi suất
        /// </summary>
        /// <param name="pphapTinhLSuat">string pphapTinhLSuat</param>
        /// <returns>phương pháp tính lãi suất</returns>
        public static string layNgonNguPPhapTinhLSuat(string pphapTinhLSuat)
        {
            if (pphapTinhLSuat == layGiaTri(PPHAP_TINH_LSUAT.BTH))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.PhuongPhapTinhLaiSuat.BTH");
            }
            else if (pphapTinhLSuat == layGiaTri(PPHAP_TINH_LSUAT.DTH))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.PhuongPhapTinhLaiSuat.DTH");
            }

            return "";
        }

        public static string layMaNgonNguPPhapTinhLSuat(string pphapTinhLSuat)
        {
            if (pphapTinhLSuat == layGiaTri(PPHAP_TINH_LSUAT.BTH))
            {
                return "U.DungChung.PhuongPhapTinhLaiSuat.BTH";
            }
            else if (pphapTinhLSuat == layGiaTri(PPHAP_TINH_LSUAT.DTH))
            {
                return "U.DungChung.PhuongPhapTinhLaiSuat.DTH";
            }

            return "";
        }

        /// <summary>
        /// Lấy ngôn ngữ hiển thị đơn vị thời gian
        /// </summary>
        /// <param name="donViThoiGian">string pphapTinhLSuat</param>
        /// <returns>đơn vị thời gian</returns>
        public static string layNgonNguDonViThoiGian(string donViThoiGian)
        {
            if (donViThoiGian == layGiaTri(TAN_SUAT.NAM))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.DonViThoiGian.NAM");
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.NGAY))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.DonViThoiGian.NGAY");
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.QUY))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.DonViThoiGian.QUY");
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.THANG))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.DonViThoiGian.THANG");
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.TUAN))
            {
                return LLanguage.SearchResourceByKey("U.DungChung.DonViThoiGian.TUAN");
            }

            return "";
        }

        public static string layMaNgonNguDonViThoiGian(string donViThoiGian)
        {
            if (donViThoiGian == layGiaTri(TAN_SUAT.NAM))
            {
                return "U.DungChung.DonViThoiGian.NAM";
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.NGAY))
            {
                return "U.DungChung.DonViThoiGian.NGAY";
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.QUY))
            {
                return "U.DungChung.DonViThoiGian.QUY";
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.THANG))
            {
                return "U.DungChung.DonViThoiGian.THANG";
            }
            else if (donViThoiGian == layGiaTri(TAN_SUAT.TUAN))
            {
                return "U.DungChung.DonViThoiGian.TUAN";
            }

            return "";
        }

        /// <summary>
        /// Trạng thái sử dụng
        /// </summary> 
        public enum TrangThaiSuDung
        {
            SU_DUNG,
            KHONG_SU_DUNG
        }
        public static string layGiaTri(this TrangThaiSuDung trangThaiSuDung)
        {
            switch (trangThaiSuDung)
            {
                case TrangThaiSuDung.SU_DUNG: return "SDU";
                case TrangThaiSuDung.KHONG_SU_DUNG: return "KDU";
                default: return "";
            }
        }

        /// <summary>
        /// Yêu cầu đổi mật khẩu trong hệ thống
        /// </summary> 
        public enum YeuCauDoiMatKhau
        {
            CHUA_THAY_DOI,
            DA_THAY_DOI
        }
        public static string layGiaTri(this YeuCauDoiMatKhau yeuCauDoiMatKhau)
        {
            switch (yeuCauDoiMatKhau)
            {
                case YeuCauDoiMatKhau.CHUA_THAY_DOI: return "CTD";
                case YeuCauDoiMatKhau.DA_THAY_DOI: return "DTD";
                default: return "";
            }
        }

        /// <summary>
        /// Phương thức phong - giải tỏa
        /// </summary>
        public enum PHUONG_THUC_PHONG_GIAI_TOA { TOAN_BO, MOT_PHAN };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PHUONG_THUC_PHONG_GIAI_TOA phuongThuc)
        {
            switch (phuongThuc)
            {
                case PHUONG_THUC_PHONG_GIAI_TOA.TOAN_BO: return "TBSD";
                case PHUONG_THUC_PHONG_GIAI_TOA.MOT_PHAN: return "MPSD";
                default: return "";
            }
        }

        /// <summary>
        /// Thời hạn phong - giải tỏa
        /// </summary>
        public enum THOI_HAN_PHONG_GIAI_TOA { CO_THOI_HAN, VO_THOI_HAN };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this THOI_HAN_PHONG_GIAI_TOA thoiHan)
        {
            switch (thoiHan)
            {
                case THOI_HAN_PHONG_GIAI_TOA.CO_THOI_HAN: return "CO_THOI_HAN";
                case THOI_HAN_PHONG_GIAI_TOA.VO_THOI_HAN: return "VO_THOI_HAN";
                default: return "";
            }
        }

        /// <summary>
        /// Đối tượng phong - giải tỏa
        /// </summary>
        public enum DOI_TUONG_PHONG_GIAI_TOA { NGUONVON, TAI_SAN, HDTD, TAI_KHOAN };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this DOI_TUONG_PHONG_GIAI_TOA loaiDoiTuong)
        {
            switch (loaiDoiTuong)
            {
                case DOI_TUONG_PHONG_GIAI_TOA.NGUONVON: return "NGUONVON";
                case DOI_TUONG_PHONG_GIAI_TOA.TAI_SAN: return "TAI_SAN";
                case DOI_TUONG_PHONG_GIAI_TOA.HDTD: return "HDTD";
                case DOI_TUONG_PHONG_GIAI_TOA.TAI_KHOAN: return "TAI_KHOAN";
                default: return "";
            }
        }

        /// <summary>
        /// Loại đối tượng khai thác dữ liệu
        /// </summary>
        public enum LoaiDoiTuong { NGUOI_SDUNG, NHOM_NGUOI_SDUNG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoaiDoiTuong loaiDoiTuong)
        {
            switch (loaiDoiTuong)
            {
                case LoaiDoiTuong.NGUOI_SDUNG: return "NSD";
                case LoaiDoiTuong.NHOM_NGUOI_SDUNG: return "NHNSD";
                default: return "";
            }
        }


        public enum SysJob { COB, D_REPORT, M_REPORT };
        public static string layGiaTri(this SysJob sysJob)
        {
            switch (sysJob)
            {
                case SysJob.COB: return "COB";
                case SysJob.D_REPORT: return "D_REPORT";
                case SysJob.M_REPORT: return "M_REPORT";
                default: return "";
            }
        }

        public enum SysJobCategory { CAT_COB, CAT_REPORT, CAT_EMAIL };
        public static string layGiaTri(this SysJobCategory sysJobCategory)
        {
            switch (sysJobCategory)
            {
                case SysJobCategory.CAT_COB: return "CAT_COB";
                case SysJobCategory.CAT_REPORT: return "CAT_REPORT";
                case SysJobCategory.CAT_EMAIL: return "CAT_EMAIL";
                default: return "";
            }
        }

        public enum SysJobPeriod { PER_ADHOC, PER_DAILY, PER_MONTHLY };
        public static string layGiaTri(this SysJobPeriod sysJobPeriod)
        {
            switch (sysJobPeriod)
            {
                case SysJobPeriod.PER_ADHOC: return "PER_ADHOC";
                case SysJobPeriod.PER_DAILY: return "PER_DAILY";
                case SysJobPeriod.PER_MONTHLY: return "PER_MONTHLY";
                default: return "";
            }
        }

        public enum SysJobType { TYPE_DEP, TYPE_INDEP };
        public static string layGiaTri(this SysJobType sysJobType)
        {
            switch (sysJobType)
            {
                case SysJobType.TYPE_DEP: return "TYPE_DEP";
                case SysJobType.TYPE_INDEP: return "TYPE_INDEP";
                default: return "";
            }
        }

        /// <summary>
        /// Loại đối tượng khai thác dữ liệu
        /// </summary>
        public enum LoaiDoiTuongSuDungTK { NGUOI_SDUNG, KHACH_HANG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoaiDoiTuongSuDungTK loaiDoiTuong)
        {
            switch (loaiDoiTuong)
            {
                case LoaiDoiTuongSuDungTK.NGUOI_SDUNG: return "NGUOI_SU_DUNG";
                case LoaiDoiTuongSuDungTK.KHACH_HANG: return "KHACH_HANG";
                default: return "";
            }
        }

        public enum LoaiKhachHang { CNHAN, DNGHIEP, TCTD, VANG_LAI, TVIEN };
        public static string layGiaTri(this LoaiKhachHang loaiKhachHang)
        {
            switch (loaiKhachHang)
            {
                case LoaiKhachHang.CNHAN: return "CNHAN";
                case LoaiKhachHang.DNGHIEP: return "DNGHIEP";
                case LoaiKhachHang.TCTD: return "TCTD";
                case LoaiKhachHang.VANG_LAI: return "VANG_LAI";
                case LoaiKhachHang.TVIEN: return "TVIEN";
                default: return "";
            }
        }
        public static LoaiKhachHang layLoaiKhachHang(string loaiKhachHang)
        {
            switch (loaiKhachHang)
            {
                case "CNHAN": return LoaiKhachHang.CNHAN;
                case "DNGHIEP": return LoaiKhachHang.DNGHIEP;
                case "TCTD": return LoaiKhachHang.TCTD;
                case "VANG_LAI": return LoaiKhachHang.VANG_LAI;
                case "TVIEN": return LoaiKhachHang.TVIEN;
                default: return LoaiKhachHang.CNHAN;
            }
        }

        /// <summary>
        /// Loại đối tượng khai thác dữ liệu
        /// </summary>
        public enum LoaiNguoiSuDung { CAP_SA, CAP_QTTW, CAP_NVTW, CAP_QTDV, CAP_NVDV };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoaiNguoiSuDung loaiNguoiSuDung)
        {
            switch (loaiNguoiSuDung)
            {
                case LoaiNguoiSuDung.CAP_SA: return "CAP_SA";
                case LoaiNguoiSuDung.CAP_QTTW: return "CAP_QTTW";
                case LoaiNguoiSuDung.CAP_NVTW: return "CAP_NVTW";
                case LoaiNguoiSuDung.CAP_QTDV: return "CAP_QTDV";
                case LoaiNguoiSuDung.CAP_NVDV: return "CAP_NVDV";
                default: return "";
            }
        }

        public static LoaiNguoiSuDung layLoaiNguoiSuDung(string loaiNguoiSuDung)
        {
            switch (loaiNguoiSuDung)
            {
                case "CAP_SA": return LoaiNguoiSuDung.CAP_SA;
                case "CAP_QTTW": return LoaiNguoiSuDung.CAP_QTTW;
                case "CAP_NVTW": return LoaiNguoiSuDung.CAP_NVTW;
                case "CAP_QTDV": return LoaiNguoiSuDung.CAP_QTDV;
                case "CAP_NVDV": return LoaiNguoiSuDung.CAP_NVDV;
                default: return LoaiNguoiSuDung.CAP_NVDV;
            }
        }

        /// <summary>
        /// Nhóm người dùng mặc định
        /// </summary>
        public enum NhomNguoiSuDung { DEF_SA, DEF_QTTW, DEF_QTDV };

        public static string layMaNhomNguoiSuDung(this NhomNguoiSuDung nhomNguoiSuDung)
        {
            switch (nhomNguoiSuDung)
            {
                case NhomNguoiSuDung.DEF_SA: return "DEF_SA";
                case NhomNguoiSuDung.DEF_QTTW: return "DEF_QTTW";
                case NhomNguoiSuDung.DEF_QTDV: return "DEF_QTDV";
                default: return "";
            }
        }

        public static int layIdNhomNguoiSuDung(this NhomNguoiSuDung nhomNguoiSuDung)
        {
            switch (nhomNguoiSuDung)
            {
                case NhomNguoiSuDung.DEF_SA: return 1;
                case NhomNguoiSuDung.DEF_QTTW: return 2;
                case NhomNguoiSuDung.DEF_QTDV: return 3;
                default: return 0;
            }
        }

        public enum LoaiThamSo { TW, DV, BC };
        public static string layGiaTri(this LoaiThamSo loaiThamSo)
        {
            switch (loaiThamSo)
            {
                case LoaiThamSo.TW: return "TW";
                case LoaiThamSo.DV: return "DV";
                case LoaiThamSo.BC: return "BC";
                default: return "";
            }
        }
        public static LoaiThamSo layLoaiThamSo(string loaiThamSo)
        {
            switch (loaiThamSo)
            {
                case "TW": return LoaiThamSo.TW;
                case "DV": return LoaiThamSo.DV;
                case "BC": return LoaiThamSo.BC;
                default: return LoaiThamSo.DV;
            }
        }

        public enum MaThamSo
        {
            // Tham số chung cho toàn hệ thống            
            TW_SOLAN_DANGNHAP_TOIDA,
            TW_DODAI_MATKHAU,
            TW_SOLUONG_BANGHI,
            TW_LYDO_CHUNG_RIENG,
            TW_DA_TIENTE,
            TW_MA_NOITE,
            TW_SONGAY_TOITHIEU_ROINHOM,
            TW_DUOC_THAMGIA_NHIEU_NHOM,
            TW_TYLE_LAINO_QUAHAN_TOIDA,
            TW_TYLE_LAI_CHAMTRA,
            TW_TYLE_LAICOCAU_KYHAN_TRANO,
            TW_SOKY_NOPTKQD_TOITHIEU,
            TW_SONGAY_TOITHIEU_TRAGOC,
            TW_SONGAY_TOITHIEU_TRALAI,
            TW_SONGAY_DAOHAN_KUOC_SAU_HDONG,
            TW_TINHLAI_NGAYDH_KUOC,
            TW_TINHLAI_NGAY_PHATVON,
            TW_TYLE_DATRANO_KUOCTRUOC,
            TW_SOLAN_GIAHAN_TOIDA,
            TW_CACHTINH_LAINO_QUAHAN,
            TW_CACHXULY_NOKEOTHEO,
            TW_MOSO_TKQD_KHI_MOHD,
            TW_XULY_VUOTHANMUC,
            TW_DUNO_TOIDA,
            TW_CHUYEN_NOQUAHAN_TUDONG,
            TW_CACH_QUANLY_VVVON,
            TW_LAISUAT_CHOVAY_TOIDA,
            TW_TYLE_DUPHONG_CHUNG,
            TW_TYLE_DUPHONG_NHOM1,
            TW_TYLE_DUPHONG_NHOM2,
            TW_TYLE_DUPHONG_NHOM3,
            TW_TYLE_DUPHONG_NHOM4,
            TW_TYLE_DUPHONG_NHOM5,
            TW_CACHTINH_PHUCAP_CUMTRUONG,
            TW_MUC_LAM_TRON_TK_TINH_LAI,
            TW_MUC_LAM_TRON_TD_TINH_LAI,
            TW_MUC_LAM_TRON_TD_CHIA_KY,
            TW_KH_MIN_TUOI,
            TW_KH_MAX_TUOI,
            TW_NGAY_GD_ROI_VAO_NGAY_NGHI,
            TW_CAN_CU_XET_QUA_HAN,
            TW_THOIDIEM_TRICHDP_CUTHE,
            TW_THOIGIAN_CHOVAY,
            TW_BIEN_DO_DCHINH_LAI_NHAP_GOC,
            TW_MA_QUOC_GIA,
            TW_SOTIEN_TKQD_TOITHIEU_KHIVAY,
            TW_TY_LE_BHIEM_TU_DUOI_3_THANG,
            TW_TY_LE_BHIEM_TU_DUOI_6_THANG,
            TW_TY_LE_BHIEM_TU_DUOI_9_THANG,
            TW_TY_LE_BHIEM_TU_DUOI_12_THANG,
            TW_TY_LE_BHIEM_TREN_12_THANG,
            TW_MUC_LAM_TRON_TD_TINH_BH,
            TW_DON_VI_SU_DUNG,
            TW_SO_TIEN_ADUNG_TINH_BAREM,
            TW_TU_DONG_TAO_THONG_TIN_KHI_DUYET_KHANG,
            TW_TINH_PHI_BHIEM_VONVAY,
            TW_TINH_SO_TIEN_KY_QUY_KHI_PHAT_VON,
            TW_PPHAP_TINH_KE_HOACH,
            TW_KTRA_LAI_SUAT_KUOC,
            TW_KTRA_DTN_HDTD,
            TW_KUOCVM_GIA_TRI_MD_MDICH_VAY,
            TW_KUOCVM_GIA_TRI_MD_NVON_VAY,
            TW_KHANG_TVIEN_SNGUOI_TDA_NHOM,
            TW_NSTL_MA_CHUC_VU_TONG_GIAM_DOC,
            TW_NSTL_MA_CHUC_VU_GIAM_DOC,
            TW_TCHAT_KY_HAN_T01,
            TW_HDTK_HIEN_THI_THUC_THU,
            TW_HDTK_MUC_PHI_TRA_TRUOC,
            TW_HDTK_BUOC_PHI_TRA_TRUOC, 
            TW_MA_LOAI_TSDB,
            TW_TY_LE_BHXH_NLD,
            TW_TY_LE_BHYT_NLD,
            TW_TY_LE_BHTN_NLD,
            TW_TY_LE_KPCD_NLD,
            TW_TY_LE_BHXH_NSDLD,
            TW_TY_LE_BHYT_NSDLD,
            TW_TY_LE_BHTN_NSDLD,
            TW_TY_LE_KPCD_NSDLD,
            TW_MUC_TINH_THUE_TNCN,
            TW_MUC_PHU_THUOC,
            TW_SO_LIEN_PHIEU_THU,
            TW_SO_LIEN_PHIEU_CHI,
            TW_SO_LIEN_PHIEU_KE_TOAN,
            TW_SO_LIEN_PHIEU_UY_NHIEM_CHI,
            TW_MUC_LAM_TRON_SO_TIEN,
            TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY,
            TW_TDVM_CACH_THU_LAI_KHI_TTOAN,
            TW_ON_CHECK_MATRIX_APPROVE,
            TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_TDAN,
            TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_MVU,
            TW_KHANG_TVIEN_SNHOM_TDA_THAM_GIA,
            TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_TDAN,
            TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_MVU,
            TW_TY_LE_HOAN_TRA_GOC,
            TW_KHOA_SO_KIEM_TRA_CHUA_DUYET,
            TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_DUYET,
            TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_GIAI_NGAN,
            TW_SO_TIEN_TUONG_TRO_HANG_THANG,
            TW_NGUONVON_MACDINH,
            TW_CHUYENDIABAN_KIEMTRASODU,
            TW_TGIAN_LAP_CHUNG_TU,
            TW_DIEM_BUTRU_GOCLAI,
            TW_HE_THONG_TKHOAN_HTHI,
            TW_KIEM_TRA_SO_DU_KHA_DUNG,
            TW_PLOAI_NO_SO_NGAY,
            TW_PLOAI_NO_TU_DONG,
            TW_DANH_SACH_CHUYEN_HOAN_TU_DONG,
            TW_KIEM_TRA_SO_VONG_VAY,
            TW_KIEM_TRA_SO_TIEN_RUT_GOC,
            TW_CACH_TINH_SO_TIEN_MO_SO_TK,
            TW_TY_LE_TINH_SO_TIEN_MO_SO_TK,
            TW_SO_TIEN_MO_SO_TK_TUYET_DOI,
            TW_TKHOAN_DANH_GIA_NGOAI_TE,
            TW_TKHOAN_THU_NHAP,
            TW_TKHOAN_CHI_PHI,
            TW_KSRR_TU_DONG,
            TW_TDVM_TAM_UNG_TRUOC_GIAI_NGAN,

            //Tham số làm tròn tín dụng tiêu dùng
            TW_TDTD_PHUONG_THUC_LAM_TRON_GOC,
            TW_TDTD_PHUONG_THUC_LAM_TRON_LAI,
            TW_TDTD_MOC_LAM_TRON,
            TW_TDTD_LAM_TRON_LEN,
            TW_TDTD_LAM_TRON_XUONG,
            TW_TDTD_PHUONG_THUC_CHIA_DEU_GOC,
            TW_TDTD_KE_HOACH_LAM_TRON_LAI,
            TW_TDTD_KE_HOACH_LAM_TRON_GOC,
            TW_TDTD_THU_TU_PHAN_BO_THU_TIEN_KY,

            TW_CHAY_TU_DONG_CUOI_NGAY,
            TW_SINH_BAO_CAO_CUOI_NGAY,
            TW_SO_NGAY_THUOC_NHOM_NO,
            TW_SO_NGAY_THUOC_PART_NO,
            TW_TOAN_TU_TINH_LSUAT_QH,
            TW_THOI_GIAN_XNHAN_MA_DXVV,
            TW_SMS_DUYET_TIN_NHAN_DEN,
            TW_SMS_DUYET_TIN_NHAN_DI,
            TW_SMS_SO_NGAY_NHAC_NO,

            // Tham số riêng cho từng đơn vị
            DV_LSUAT_TRAN_CO_KY_HAN,
            DV_LSUAT_TRAN_KHONG_KY_HAN,
            DV_SO_DU_TINH_LAI_KHI_RUT_GOC,
            DV_CACH_HACH_TOAN_THOAI_CHI,
            DV_PHAN_BO_DOANH_THU,
            DV_DU_THU_NGOAI_BANG,
            DV_DU_THU_DEN_NGAY,
            DV_NGAY_GIUA_THANG,
            DV_CACH_TINH_LAI_TRUOC_HAN,
            DV_LOAI_DVI_KET_CHUYEN,
            DV_THOI_GIAN_CTU_GHI_SO,
            DV_LOAI_DTUONG_NHANSU,
            DV_THU_LAI_HACH_TOAN_TUNG,
            DV_KIEM_TRA_SO_DU_TAT_TOAN_T01,
            DV_SAN_PHAM_MAC_DINH_T01,
            DV_SAN_PHAM_MAC_DINH_T02,
            DV_SAN_PHAM_MAC_DINH_HDTDVM,
            DV_PHAN_BO_DOANH_THU_GNGAN,
            DV_SO_NGAY_TOI_THIEU_TAO_KY_TRA_NO,
            DV_NGAY_KET_THUC_NAM_TCHINH,
            DV_TIEN_TO_SMS,
            DV_NGON_NGU_SMS,

            TW_TY_LE_BHIEM_TU_DUOI_4_THANG,
            TW_TY_LE_BHIEM_TU_DUOI_5_THANG,
            TW_TY_LE_BHIEM_TU_DUOI_7_THANG,
            TW_TY_LE_BHIEM_TU_DUOI_8_THANG,
                       
            // Tham số báo cáo cho toàn hệ thống
            DEFAULT,

            //Tham so chi thi dao han hach toan truoc hoac sau ngay nghi
            TW_CHI_THI_DAO_HAN_NGAY_NGHI,
            TW_MUC_LAM_TRON_TD_TINH_LAI_DU_THU,

            //Tham so lai suat san cua tien gui
            DV_LSUAT_SAN_CO_KY_HAN,
            DV_LSUAT_SAN_KHONG_KY_HAN,

            //Tham so kiem tra han muc
            DV_KIEM_TRA_HAN_MUC

        };
        public static string layGiaTri(this MaThamSo maThamSo)
        {
            switch (maThamSo)
            {
                // Tham số chung cho toàn hệ thống
                case MaThamSo.TW_SOLAN_DANGNHAP_TOIDA: return "TW_SOLAN_DANGNHAP_TOIDA";
                case MaThamSo.TW_DODAI_MATKHAU: return "TW_DODAI_MATKHAU";
                case MaThamSo.TW_SOLUONG_BANGHI: return "TW_SOLUONG_BANGHI";
                case MaThamSo.TW_LYDO_CHUNG_RIENG: return "TW_LYDO_CHUNG_RIENG";
                case MaThamSo.TW_DA_TIENTE: return "TW_DA_TIENTE";
                case MaThamSo.TW_MA_NOITE: return "TW_MA_NOITE";
                case MaThamSo.TW_SONGAY_TOITHIEU_ROINHOM: return "TW_SONGAY_TOITHIEU_ROINHOM";
                case MaThamSo.TW_DUOC_THAMGIA_NHIEU_NHOM: return "TW_DUOC_THAMGIA_NHIEU_NHOM";
                case MaThamSo.TW_TYLE_LAINO_QUAHAN_TOIDA: return "TW_TYLE_LAINO_QUAHAN_TOIDA";
                case MaThamSo.TW_TYLE_LAI_CHAMTRA: return "TW_TYLE_LAI_CHAMTRA";
                case MaThamSo.TW_TYLE_LAICOCAU_KYHAN_TRANO: return "TW_TYLE_LAICOCAU_KYHAN_TRANO";
                case MaThamSo.TW_SOKY_NOPTKQD_TOITHIEU: return "TW_SOKY_NOPTKQD_TOITHIEU";
                case MaThamSo.TW_SONGAY_TOITHIEU_TRAGOC: return "TW_SONGAY_TOITHIEU_TRAGOC";
                case MaThamSo.TW_SONGAY_TOITHIEU_TRALAI: return "TW_SONGAY_TOITHIEU_TRALAI";
                case MaThamSo.TW_SONGAY_DAOHAN_KUOC_SAU_HDONG: return "TW_SONGAY_DAOHAN_KUOC_SAU_HDONG";
                case MaThamSo.TW_TINHLAI_NGAYDH_KUOC: return "TW_TINHLAI_NGAYDH_KUOC";
                case MaThamSo.TW_TINHLAI_NGAY_PHATVON: return "TW_TINHLAI_NGAY_PHATVON";
                case MaThamSo.TW_TYLE_DATRANO_KUOCTRUOC: return "TW_TYLE_DATRANO_KUOCTRUOC";
                case MaThamSo.TW_SOLAN_GIAHAN_TOIDA: return "TW_SOLAN_GIAHAN_TOIDA";
                case MaThamSo.TW_CACHTINH_LAINO_QUAHAN: return "TW_CACHTINH_LAINO_QUAHAN";
                case MaThamSo.TW_CACHXULY_NOKEOTHEO: return "TW_CACHXULY_NOKEOTHEO";
                case MaThamSo.TW_MOSO_TKQD_KHI_MOHD: return "TW_MOSO_TKQD_KHI_MOHD";
                case MaThamSo.TW_XULY_VUOTHANMUC: return "TW_XULY_VUOTHANMUC";
                case MaThamSo.TW_DUNO_TOIDA: return "TW_DUNO_TOIDA";
                case MaThamSo.TW_CHUYEN_NOQUAHAN_TUDONG: return "TW_CHUYEN_NOQUAHAN_TUDONG";
                case MaThamSo.TW_CACH_QUANLY_VVVON: return "TW_CACH_QUANLY_VVVON";
                case MaThamSo.TW_LAISUAT_CHOVAY_TOIDA: return "TW_LAISUAT_CHOVAY_TOIDA";
                case MaThamSo.TW_TYLE_DUPHONG_CHUNG: return "TW_TYLE_DUPHONG_CHUNG";
                case MaThamSo.TW_TYLE_DUPHONG_NHOM1: return "TW_TYLE_DUPHONG_NHOM1";
                case MaThamSo.TW_TYLE_DUPHONG_NHOM2: return "TW_TYLE_DUPHONG_NHOM2";
                case MaThamSo.TW_TYLE_DUPHONG_NHOM3: return "TW_TYLE_DUPHONG_NHOM3";
                case MaThamSo.TW_TYLE_DUPHONG_NHOM4: return "TW_TYLE_DUPHONG_NHOM4";
                case MaThamSo.TW_TYLE_DUPHONG_NHOM5: return "TW_TYLE_DUPHONG_NHOM5";
                case MaThamSo.TW_CACHTINH_PHUCAP_CUMTRUONG: return "TW_CACHTINH_PHUCAP_CUMTRUONG";
                case MaThamSo.TW_MUC_LAM_TRON_TK_TINH_LAI: return "TW_MUC_LAM_TRON_TK_TINH_LAI";
                case MaThamSo.TW_MUC_LAM_TRON_TD_TINH_LAI: return "TW_MUC_LAM_TRON_TD_TINH_LAI";
                case MaThamSo.TW_MUC_LAM_TRON_TD_CHIA_KY: return "TW_MUC_LAM_TRON_TD_CHIA_KY";
                case MaThamSo.TW_KH_MIN_TUOI: return "TW_KH_MIN_TUOI";
                case MaThamSo.TW_KH_MAX_TUOI: return "TW_KH_MAX_TUOI";
                case MaThamSo.TW_NGAY_GD_ROI_VAO_NGAY_NGHI: return "TW_NGAY_GD_ROI_VAO_NGAY_NGHI";
                case MaThamSo.TW_CAN_CU_XET_QUA_HAN: return "TW_CAN_CU_XET_QUA_HAN";
                case MaThamSo.TW_THOIDIEM_TRICHDP_CUTHE: return "TW_THOIDIEM_TRICHDP_CUTHE";
                case MaThamSo.TW_THOIGIAN_CHOVAY: return "TW_THOIGIAN_CHOVAY";
                case MaThamSo.TW_BIEN_DO_DCHINH_LAI_NHAP_GOC: return "TW_BIEN_DO_DCHINH_LAI_NHAP_GOC";
                case MaThamSo.TW_MA_QUOC_GIA: return "TW_MA_QUOC_GIA";
                case MaThamSo.TW_SOTIEN_TKQD_TOITHIEU_KHIVAY: return "TW_SOTIEN_TKQD_TOITHIEU_KHIVAY";
                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_3_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_3_THANG";
                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_6_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_6_THANG";
                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_9_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_9_THANG";
                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_12_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_12_THANG";
                case MaThamSo.TW_TY_LE_BHIEM_TREN_12_THANG: return "TW_TY_LE_BHIEM_TREN_12_THANG";
                case MaThamSo.TW_MUC_LAM_TRON_TD_TINH_BH: return "TW_MUC_LAM_TRON_TD_TINH_BH";
                case MaThamSo.TW_DON_VI_SU_DUNG: return "TW_DON_VI_SU_DUNG";
                case MaThamSo.TW_SO_TIEN_ADUNG_TINH_BAREM: return "TW_SO_TIEN_ADUNG_TINH_BAREM";
                case MaThamSo.TW_TU_DONG_TAO_THONG_TIN_KHI_DUYET_KHANG: return "TW_TU_DONG_TAO_THONG_TIN_KHI_DUYET_KHANG";
                case MaThamSo.TW_TINH_PHI_BHIEM_VONVAY: return "TW_TINH_PHI_BHIEM_VONVAY";
                case MaThamSo.TW_TINH_SO_TIEN_KY_QUY_KHI_PHAT_VON: return "TW_TINH_SO_TIEN_KY_QUY_KHI_PHAT_VON";
                case MaThamSo.TW_PPHAP_TINH_KE_HOACH: return "TW_PPHAP_TINH_KE_HOACH";
                case MaThamSo.TW_KTRA_DTN_HDTD: return "TW_KTRA_DTN_HDTD";
                case MaThamSo.TW_KTRA_LAI_SUAT_KUOC: return "TW_KTRA_LAI_SUAT_KUOC";
                case MaThamSo.TW_KUOCVM_GIA_TRI_MD_MDICH_VAY: return "TW_KUOCVM_GIA_TRI_MD_MDICH_VAY";
                case MaThamSo.TW_KUOCVM_GIA_TRI_MD_NVON_VAY: return "TW_KUOCVM_GIA_TRI_MD_NVON_VAY";
                case MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM: return "TW_KHANG_TVIEN_SNGUOI_TDA_NHOM";
                case MaThamSo.TW_NSTL_MA_CHUC_VU_TONG_GIAM_DOC: return "TW_NSTL_MA_CHUC_VU_TONG_GIAM_DOC";
                case MaThamSo.TW_NSTL_MA_CHUC_VU_GIAM_DOC: return "TW_NSTL_MA_CHUC_VU_GIAM_DOC";
                case MaThamSo.TW_TCHAT_KY_HAN_T01: return "TW_TCHAT_KY_HAN_T01";
                case MaThamSo.TW_HDTK_HIEN_THI_THUC_THU: return "TW_HDTK_HIEN_THI_THUC_THU";
                case MaThamSo.TW_HDTK_MUC_PHI_TRA_TRUOC: return "TW_HDTK_MUC_PHI_TRA_TRUOC";
                case MaThamSo.TW_HDTK_BUOC_PHI_TRA_TRUOC: return "TW_HDTK_BUOC_PHI_TRA_TRUOC";
                case MaThamSo.TW_MA_LOAI_TSDB: return "TW_MA_LOAI_TSDB";
                case MaThamSo.TW_TY_LE_BHXH_NLD: return "TW_TY_LE_BHXH_NLD";
                case MaThamSo.TW_TY_LE_BHYT_NLD: return "TW_TY_LE_BHYT_NLD";
                case MaThamSo.TW_TY_LE_BHTN_NLD: return "TW_TY_LE_BHTN_NLD";
                case MaThamSo.TW_TY_LE_KPCD_NLD: return "TW_TY_LE_KPCD_NLD";
                case MaThamSo.TW_TY_LE_BHXH_NSDLD: return "TW_TY_LE_BHXH_NSDLD";
                case MaThamSo.TW_TY_LE_BHYT_NSDLD: return "TW_TY_LE_BHYT_NSDLD";
                case MaThamSo.TW_TY_LE_BHTN_NSDLD: return "TW_TY_LE_BHTN_NSDLD";
                case MaThamSo.TW_TY_LE_KPCD_NSDLD: return "TW_TY_LE_KPCD_NSDLD";
                case MaThamSo.TW_MUC_TINH_THUE_TNCN: return "TW_MUC_TINH_THUE_TNCN";
                case MaThamSo.TW_MUC_PHU_THUOC: return "TW_MUC_PHU_THUOC";
                case MaThamSo.TW_SO_LIEN_PHIEU_CHI: return "TW_SO_LIEN_PHIEU_CHI";
                case MaThamSo.TW_SO_LIEN_PHIEU_KE_TOAN: return "TW_SO_LIEN_PHIEU_KE_TOAN";
                case MaThamSo.TW_SO_LIEN_PHIEU_THU: return "TW_SO_LIEN_PHIEU_THU";
                case MaThamSo.TW_SO_LIEN_PHIEU_UY_NHIEM_CHI: return "TW_SO_LIEN_PHIEU_UY_NHIEM_CHI";
                case MaThamSo.TW_MUC_LAM_TRON_SO_TIEN: return "TW_MUC_LAM_TRON_SO_TIEN";
                case MaThamSo.TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY: return "TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY";
                case MaThamSo.TW_TDVM_CACH_THU_LAI_KHI_TTOAN: return "TW_TDVM_CACH_THU_LAI_KHI_TTOAN";
                case MaThamSo.TW_ON_CHECK_MATRIX_APPROVE: return "TW_ON_CHECK_MATRIX_APPROVE";
                case MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_TDAN: return "TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_TDAN";
                case MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_MVU: return "TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_MVU";
                case MaThamSo.TW_KHANG_TVIEN_SNHOM_TDA_THAM_GIA: return "TW_KHANG_TVIEN_SNHOM_TDA_THAM_GIA";
                case MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_TDAN: return "TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_TDAN";
                case MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_MVU: return "TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_MVU";
                case MaThamSo.TW_TY_LE_HOAN_TRA_GOC: return "TW_TY_LE_HOAN_TRA_GOC";
                case MaThamSo.TW_SO_TIEN_TUONG_TRO_HANG_THANG: return "TW_SO_TIEN_TUONG_TRO_HANG_THANG";
                case MaThamSo.TW_NGUONVON_MACDINH: return "TW_NGUONVON_MACDINH";
                case MaThamSo.TW_CHUYENDIABAN_KIEMTRASODU: return "TW_CHUYENDIABAN_KIEMTRASODU";
                case MaThamSo.TW_TGIAN_LAP_CHUNG_TU: return "TW_TGIAN_LAP_CHUNG_TU";
                case MaThamSo.TW_DIEM_BUTRU_GOCLAI: return "TW_DIEM_BUTRU_GOCLAI";
                case MaThamSo.TW_HE_THONG_TKHOAN_HTHI: return "TW_HE_THONG_TKHOAN_HTHI";
                case MaThamSo.TW_KIEM_TRA_SO_DU_KHA_DUNG: return "TW_KIEM_TRA_SO_DU_KHA_DUNG";
                case MaThamSo.TW_KHOA_SO_KIEM_TRA_CHUA_DUYET: return "TW_KHOA_SO_KIEM_TRA_CHUA_DUYET";
                case MaThamSo.TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_DUYET: return "TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_DUYET";
                case MaThamSo.TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_GIAI_NGAN: return "TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_GIAI_NGAN";
                case MaThamSo.TW_PLOAI_NO_SO_NGAY: return "TW_PLOAI_NO_SO_NGAY";
                case MaThamSo.TW_PLOAI_NO_TU_DONG: return "TW_PLOAI_NO_TU_DONG";
                case MaThamSo.TW_DANH_SACH_CHUYEN_HOAN_TU_DONG: return "TW_DANH_SACH_CHUYEN_HOAN_TU_DONG";
                case MaThamSo.TW_KIEM_TRA_SO_VONG_VAY: return "TW_KIEM_TRA_SO_VONG_VAY";
                case MaThamSo.TW_KIEM_TRA_SO_TIEN_RUT_GOC: return "TW_KIEM_TRA_SO_TIEN_RUT_GOC";
                case MaThamSo.TW_CACH_TINH_SO_TIEN_MO_SO_TK: return "TW_CACH_TINH_SO_TIEN_MO_SO_TK";
                case MaThamSo.TW_TY_LE_TINH_SO_TIEN_MO_SO_TK: return "TW_TY_LE_TINH_SO_TIEN_MO_SO_TK";
                case MaThamSo.TW_SO_TIEN_MO_SO_TK_TUYET_DOI: return "TW_SO_TIEN_MO_SO_TK_TUYET_DOI";
                case MaThamSo.TW_TKHOAN_DANH_GIA_NGOAI_TE: return "TW_TKHOAN_DANH_GIA_NGOAI_TE";
                case MaThamSo.TW_TKHOAN_THU_NHAP: return "TW_TKHOAN_THU_NHAP";
                case MaThamSo.TW_TKHOAN_CHI_PHI: return "TW_TKHOAN_CHI_PHI";
                case MaThamSo.TW_KSRR_TU_DONG: return "TW_KSRR_TU_DONG";
                case MaThamSo.TW_TDVM_TAM_UNG_TRUOC_GIAI_NGAN: return "TW_TDVM_TAM_UNG_TRUOC_GIAI_NGAN";

                // Tham số làm tròn tín dụng tiêu dùng

                case MaThamSo.TW_TDTD_PHUONG_THUC_LAM_TRON_GOC: return "TW_TDTD_PHUONG_THUC_LAM_TRON_GOC";
                case MaThamSo.TW_TDTD_PHUONG_THUC_LAM_TRON_LAI: return "TW_TDTD_PHUONG_THUC_LAM_TRON_LAI";
                case MaThamSo.TW_TDTD_MOC_LAM_TRON: return "TW_TDTD_MOC_LAM_TRON";
                case MaThamSo.TW_TDTD_LAM_TRON_LEN: return "TW_TDTD_LAM_TRON_LEN";
                case MaThamSo.TW_TDTD_LAM_TRON_XUONG: return "TW_TDTD_LAM_TRON_XUONG";
                case MaThamSo.TW_TDTD_PHUONG_THUC_CHIA_DEU_GOC: return "TW_TDTD_PHUONG_THUC_CHIA_DEU_GOC";
                case MaThamSo.TW_TDTD_KE_HOACH_LAM_TRON_LAI: return "TW_TDTD_KE_HOACH_LAM_TRON_LAI";
                case MaThamSo.TW_TDTD_KE_HOACH_LAM_TRON_GOC: return "TW_TDTD_KE_HOACH_LAM_TRON_GOC";
                case MaThamSo.TW_TDTD_THU_TU_PHAN_BO_THU_TIEN_KY: return "TW_TDTD_THU_TU_PHAN_BO_THU_TIEN_KY";

                case MaThamSo.TW_CHAY_TU_DONG_CUOI_NGAY: return "TW_CHAY_TU_DONG_CUOI_NGAY";
                case MaThamSo.TW_SINH_BAO_CAO_CUOI_NGAY: return "TW_SINH_BAO_CAO_CUOI_NGAY";
                case MaThamSo.TW_SO_NGAY_THUOC_NHOM_NO: return "TW_SO_NGAY_THUOC_NHOM_NO";
                case MaThamSo.TW_SO_NGAY_THUOC_PART_NO: return "TW_SO_NGAY_THUOC_PART_NO";
                case MaThamSo.TW_TOAN_TU_TINH_LSUAT_QH: return "TW_TOAN_TU_TINH_LSUAT_QH";
                case MaThamSo.TW_THOI_GIAN_XNHAN_MA_DXVV: return "TW_THOI_GIAN_XNHAN_MA_DXVV";
                case MaThamSo.TW_SMS_DUYET_TIN_NHAN_DEN: return "TW_SMS_DUYET_TIN_NHAN_DEN";
                case MaThamSo.TW_SMS_DUYET_TIN_NHAN_DI: return "TW_SMS_DUYET_TIN_NHAN_DI";
                case MaThamSo.TW_SMS_SO_NGAY_NHAC_NO: return "TW_SMS_SO_NGAY_NHAC_NO";

                // Tham số riêng cho từng đơn vị 
                case MaThamSo.DV_LSUAT_TRAN_CO_KY_HAN: return "DV_LSUAT_TRAN_CO_KY_HAN";
                case MaThamSo.DV_LSUAT_TRAN_KHONG_KY_HAN: return "DV_LSUAT_TRAN_KHONG_KY_HAN";
                case MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC: return "DV_SO_DU_TINH_LAI_KHI_RUT_GOC";
                case MaThamSo.DV_CACH_HACH_TOAN_THOAI_CHI: return "DV_CACH_HACH_TOAN_THOAI_CHI";
                case MaThamSo.DV_PHAN_BO_DOANH_THU: return "DV_PHAN_BO_DOANH_THU";
                case MaThamSo.DV_PHAN_BO_DOANH_THU_GNGAN: return "DV_PHAN_BO_DOANH_THU_GNGAN";
                case MaThamSo.DV_DU_THU_NGOAI_BANG: return "DV_DU_THU_NGOAI_BANG";
                case MaThamSo.DV_DU_THU_DEN_NGAY: return "DV_DU_THU_DEN_NGAY";
                case MaThamSo.DV_NGAY_GIUA_THANG: return "DV_NGAY_GIUA_THANG";
                case MaThamSo.DV_CACH_TINH_LAI_TRUOC_HAN: return "DV_CACH_TINH_LAI_TRUOC_HAN";
                case MaThamSo.DV_LOAI_DVI_KET_CHUYEN: return "DV_LOAI_DVI_KET_CHUYEN";
                case MaThamSo.DV_THOI_GIAN_CTU_GHI_SO: return "DV_THOI_GIAN_CTU_GHI_SO";
                case MaThamSo.DV_LOAI_DTUONG_NHANSU: return "DV_LOAI_DTUONG_NHANSU";
                case MaThamSo.DV_THU_LAI_HACH_TOAN_TUNG: return "DV_THU_LAI_HACH_TOAN_TUNG";
                case MaThamSo.DV_KIEM_TRA_SO_DU_TAT_TOAN_T01: return "DV_KIEM_TRA_SO_DU_TAT_TOAN_T01";
                case MaThamSo.DV_SAN_PHAM_MAC_DINH_T01: return "DV_SAN_PHAM_MAC_DINH_T01";
                case MaThamSo.DV_SAN_PHAM_MAC_DINH_T02: return "DV_SAN_PHAM_MAC_DINH_T02";
                case MaThamSo.DV_SAN_PHAM_MAC_DINH_HDTDVM: return "DV_SAN_PHAM_MAC_DINH_HDTDVM";
                case MaThamSo.DV_SO_NGAY_TOI_THIEU_TAO_KY_TRA_NO: return "DV_SO_NGAY_TOI_THIEU_TAO_KY_TRA_NO";
                case MaThamSo.DV_NGAY_KET_THUC_NAM_TCHINH: return "DV_NGAY_KET_THUC_NAM_TCHINH";
                case MaThamSo.DV_TIEN_TO_SMS: return "DV_TIEN_TO_SMS";
                case MaThamSo.DV_NGON_NGU_SMS: return "DV_NGON_NGU_SMS";

                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_4_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_4_THANG";
                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_5_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_5_THANG";
                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_7_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_7_THANG";
                case MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_8_THANG: return "TW_TY_LE_BHIEM_TU_DUOI_8_THANG";

                case MaThamSo.TW_CHI_THI_DAO_HAN_NGAY_NGHI: return "TW_CHI_THI_DAO_HAN_NGAY_NGHI";
                case MaThamSo.TW_MUC_LAM_TRON_TD_TINH_LAI_DU_THU: return "TW_MUC_LAM_TRON_TD_TINH_LAI_DU_THU";

                case MaThamSo.DV_LSUAT_SAN_CO_KY_HAN: return "DV_LSUAT_SAN_CO_KY_HAN";
                case MaThamSo.DV_LSUAT_SAN_KHONG_KY_HAN: return "DV_LSUAT_SAN_KHONG_KY_HAN";

                case MaThamSo.DV_KIEM_TRA_HAN_MUC: return "DV_KIEM_TRA_HAN_MUC";

                // Tham số báo cáo cho toàn hệ thống
                default: return "DEFAULT";
            }
        }
        public static MaThamSo layMaThamSo(string maThamSo)
        {
            switch (maThamSo)
            {
                // Tham số chung cho toàn hệ thống
                case "TW_SOLAN_DANGNHAP_TOIDA": return MaThamSo.TW_SOLAN_DANGNHAP_TOIDA;
                case "TW_DODAI_MATKHAU": return MaThamSo.TW_DODAI_MATKHAU;
                case "TW_SOLUONG_BANGHI": return MaThamSo.TW_SOLUONG_BANGHI;
                case "TW_LYDO_CHUNG_RIENG": return MaThamSo.TW_LYDO_CHUNG_RIENG;
                case "TW_DA_TIENTE": return MaThamSo.TW_DA_TIENTE;
                case "TW_MA_NOITE": return MaThamSo.TW_MA_NOITE;
                case "TW_SONGAY_TOITHIEU_ROINHOM": return MaThamSo.TW_SONGAY_TOITHIEU_ROINHOM;
                case "TW_DUOC_THAMGIA_NHIEU_NHOM": return MaThamSo.TW_DUOC_THAMGIA_NHIEU_NHOM;
                case "TW_TYLE_LAINO_QUAHAN_TOIDA": return MaThamSo.TW_TYLE_LAINO_QUAHAN_TOIDA;
                case "TW_TYLE_LAI_CHAMTRA": return MaThamSo.TW_TYLE_LAI_CHAMTRA;
                case "TW_TYLE_LAICOCAU_KYHAN_TRANO": return MaThamSo.TW_TYLE_LAICOCAU_KYHAN_TRANO;
                case "TW_SOKY_NOPTKQD_TOITHIEU": return MaThamSo.TW_SOKY_NOPTKQD_TOITHIEU;
                case "TW_SONGAY_TOITHIEU_TRAGOC": return MaThamSo.TW_SONGAY_TOITHIEU_TRAGOC;
                case "TW_SONGAY_TOITHIEU_TRALAI": return MaThamSo.TW_SONGAY_TOITHIEU_TRALAI;
                case "TW_SONGAY_DAOHAN_KUOC_SAU_HDONG": return MaThamSo.TW_SONGAY_DAOHAN_KUOC_SAU_HDONG;
                case "TW_TINHLAI_NGAYDH_KUOC": return MaThamSo.TW_TINHLAI_NGAYDH_KUOC;
                case "TW_TINHLAI_NGAY_PHATVON": return MaThamSo.TW_TINHLAI_NGAY_PHATVON;
                case "TW_TYLE_DATRANO_KUOCTRUOC": return MaThamSo.TW_TYLE_DATRANO_KUOCTRUOC;
                case "TW_SOLAN_GIAHAN_TOIDA": return MaThamSo.TW_SOLAN_GIAHAN_TOIDA;
                case "TW_CACHTINH_LAINO_QUAHAN": return MaThamSo.TW_CACHTINH_LAINO_QUAHAN;
                case "TW_CACHXULY_NOKEOTHEO": return MaThamSo.TW_CACHXULY_NOKEOTHEO;
                case "TW_MOSO_TKQD_KHI_MOHD": return MaThamSo.TW_MOSO_TKQD_KHI_MOHD;
                case "TW_XULY_VUOTHANMUC": return MaThamSo.TW_XULY_VUOTHANMUC;
                case "TW_DUNO_TOIDA": return MaThamSo.TW_DUNO_TOIDA;
                case "TW_CHUYEN_NOQUAHAN_TUDONG": return MaThamSo.TW_CHUYEN_NOQUAHAN_TUDONG;
                case "TW_CACH_QUANLY_VVVON": return MaThamSo.TW_CACH_QUANLY_VVVON;
                case "TW_LAISUAT_CHOVAY_TOIDA": return MaThamSo.TW_LAISUAT_CHOVAY_TOIDA;
                case "TW_TYLE_DUPHONG_CHUNG": return MaThamSo.TW_TYLE_DUPHONG_CHUNG;
                case "TW_TYLE_DUPHONG_NHOM1": return MaThamSo.TW_TYLE_DUPHONG_NHOM1;
                case "TW_TYLE_DUPHONG_NHOM2": return MaThamSo.TW_TYLE_DUPHONG_NHOM2;
                case "TW_TYLE_DUPHONG_NHOM3": return MaThamSo.TW_TYLE_DUPHONG_NHOM3;
                case "TW_TYLE_DUPHONG_NHOM4": return MaThamSo.TW_TYLE_DUPHONG_NHOM4;
                case "TW_TYLE_DUPHONG_NHOM5": return MaThamSo.TW_TYLE_DUPHONG_NHOM5;
                case "TW_CACHTINH_PHUCAP_CUMTRUONG": return MaThamSo.TW_CACHTINH_PHUCAP_CUMTRUONG;
                case "TW_MUC_LAM_TRON_TK_TINH_LAI": return MaThamSo.TW_MUC_LAM_TRON_TK_TINH_LAI;
                case "TW_MUC_LAM_TRON_TD_TINH_LAI": return MaThamSo.TW_MUC_LAM_TRON_TD_TINH_LAI;
                case "TW_MUC_LAM_TRON_TD_CHIA_KY": return MaThamSo.TW_MUC_LAM_TRON_TD_CHIA_KY;
                case "TW_KH_MIN_TUOI": return MaThamSo.TW_KH_MIN_TUOI;
                case "TW_KH_MAX_TUOI": return MaThamSo.TW_KH_MAX_TUOI;
                case "TW_NGAY_GD_ROI_VAO_NGAY_NGHI": return MaThamSo.TW_NGAY_GD_ROI_VAO_NGAY_NGHI;
                case "TW_CAN_CU_XET_QUA_HAN": return MaThamSo.TW_CAN_CU_XET_QUA_HAN;
                case "TW_THOIDIEM_TRICHDP_CUTHE": return MaThamSo.TW_THOIDIEM_TRICHDP_CUTHE;
                case "TW_THOIGIAN_CHOVAY": return MaThamSo.TW_THOIGIAN_CHOVAY;
                case "TW_BIEN_DO_DCHINH_LAI_NHAP_GOC": return MaThamSo.TW_BIEN_DO_DCHINH_LAI_NHAP_GOC;
                case "TW_MA_QUOC_GIA": return MaThamSo.TW_MA_QUOC_GIA;
                case "TW_SOTIEN_TKQD_TOITHIEU_KHIVAY": return MaThamSo.TW_SOTIEN_TKQD_TOITHIEU_KHIVAY;
                case "TW_TY_LE_BHIEM_TU_DUOI_3_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_3_THANG;
                case "TW_TY_LE_BHIEM_TU_DUOI_6_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_6_THANG;
                case "TW_TY_LE_BHIEM_TU_DUOI_9_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_9_THANG;
                case "TW_TY_LE_BHIEM_TU_DUOI_12_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_12_THANG;
                case "TW_TY_LE_BHIEM_TREN_12_THANG": return MaThamSo.TW_TY_LE_BHIEM_TREN_12_THANG;
                case "TW_MUC_LAM_TRON_TD_TINH_BH": return MaThamSo.TW_MUC_LAM_TRON_TD_TINH_BH;
                case "TW_DON_VI_SU_DUNG": return MaThamSo.TW_DON_VI_SU_DUNG;
                case "TW_SO_TIEN_ADUNG_TINH_BAREM": return MaThamSo.TW_SO_TIEN_ADUNG_TINH_BAREM;
                case "TW_TU_DONG_TAO_THONG_TIN_KHI_DUYET_KHANG": return MaThamSo.TW_TU_DONG_TAO_THONG_TIN_KHI_DUYET_KHANG;
                case "TW_TINH_PHI_BHIEM_VONVAY": return MaThamSo.TW_TINH_PHI_BHIEM_VONVAY;
                case "TW_TINH_SO_TIEN_KY_QUY_KHI_PHAT_VON": return MaThamSo.TW_TINH_SO_TIEN_KY_QUY_KHI_PHAT_VON;
                case "TW_PPHAP_TINH_KE_HOACH": return MaThamSo.TW_PPHAP_TINH_KE_HOACH;
                case "TW_KTRA_LAI_SUAT_KUOC": return MaThamSo.TW_KTRA_LAI_SUAT_KUOC;
                case "TW_KTRA_DTN_HDTD": return MaThamSo.TW_KTRA_DTN_HDTD;
                case "TW_KUOCVM_GIA_TRI_MD_MDICH_VAY": return MaThamSo.TW_KUOCVM_GIA_TRI_MD_MDICH_VAY;
                case "TW_KUOCVM_GIA_TRI_MD_NVON_VAY": return MaThamSo.TW_KUOCVM_GIA_TRI_MD_NVON_VAY;
                case "TW_KHANG_TVIEN_SNGUOI_TDA_NHOM": return MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM;
                case "TW_NSTL_MA_CHUC_VU_TONG_GIAM_DOC": return MaThamSo.TW_NSTL_MA_CHUC_VU_TONG_GIAM_DOC;
                case "TW_NSTL_MA_CHUC_VU_GIAM_DOC": return MaThamSo.TW_NSTL_MA_CHUC_VU_GIAM_DOC;
                case "TW_TCHAT_KY_HAN_T01": return MaThamSo.TW_TCHAT_KY_HAN_T01;
                case "TW_HDTK_HIEN_THI_THUC_THU": return MaThamSo.TW_HDTK_HIEN_THI_THUC_THU;
                case "TW_HDTK_MUC_PHI_TRA_TRUOC": return MaThamSo.TW_HDTK_MUC_PHI_TRA_TRUOC;
                case "TW_HDTK_BUOC_PHI_TRA_TRUOC": return MaThamSo.TW_HDTK_BUOC_PHI_TRA_TRUOC;
                case "TW_MA_LOAI_TSDB": return MaThamSo.TW_MA_LOAI_TSDB;
                case "TW_TY_LE_BHXH_NLD": return MaThamSo.TW_TY_LE_BHXH_NLD;
                case "TW_TY_LE_BHYT_NLD": return MaThamSo.TW_TY_LE_BHYT_NLD;
                case "TW_TY_LE_BHTN_NLD": return MaThamSo.TW_TY_LE_BHTN_NLD;
                case "TW_TY_LE_KPCD_NLD": return MaThamSo.TW_TY_LE_KPCD_NLD;
                case "TW_TY_LE_BHXH_NSDLD": return MaThamSo.TW_TY_LE_BHXH_NSDLD;
                case "TW_TY_LE_BHYT_NSDLD": return MaThamSo.TW_TY_LE_BHYT_NSDLD;
                case "TW_TY_LE_BHTN_NSDLD": return MaThamSo.TW_TY_LE_BHTN_NSDLD;
                case "TW_TY_LE_KPCD_NSDLD": return MaThamSo.TW_TY_LE_KPCD_NSDLD;
                case "TW_MUC_TINH_THUE_TNCN": return MaThamSo.TW_MUC_TINH_THUE_TNCN;
                case "TW_MUC_PHU_THUOC": return MaThamSo.TW_MUC_PHU_THUOC;
                case "TW_SO_LIEN_PHIEU_CHI": return MaThamSo.TW_SO_LIEN_PHIEU_CHI;
                case "TW_SO_LIEN_PHIEU_KE_TOAN": return MaThamSo.TW_SO_LIEN_PHIEU_KE_TOAN;
                case "TW_SO_LIEN_PHIEU_THU": return MaThamSo.TW_SO_LIEN_PHIEU_THU;
                case "TW_SO_LIEN_PHIEU_UY_NHIEM_CHI": return MaThamSo.TW_SO_LIEN_PHIEU_UY_NHIEM_CHI;
                case "TW_MUC_LAM_TRON_SO_TIEN": return MaThamSo.TW_MUC_LAM_TRON_SO_TIEN;
                case "TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY": return MaThamSo.TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY;
                case "TW_TDVM_CACH_THU_LAI_KHI_TTOAN": return MaThamSo.TW_TDVM_CACH_THU_LAI_KHI_TTOAN;
                case "TW_ON_CHECK_MATRIX_APPROVE": return MaThamSo.TW_ON_CHECK_MATRIX_APPROVE;
                case "TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_TDAN": return MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_TDAN;
                case "TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_MVU": return MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_MVU;
                case "TW_KHANG_TVIEN_SNHOM_TDA_THAM_GIA": return MaThamSo.TW_KHANG_TVIEN_SNHOM_TDA_THAM_GIA;
                case "TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_TDAN": return MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_TDAN;
                case "TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_MVU": return MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_MVU;
                case "TW_TY_LE_HOAN_TRA_GOC": return MaThamSo.TW_TY_LE_HOAN_TRA_GOC;
                case "TW_SO_TIEN_TUONG_TRO_HANG_THANG": return MaThamSo.TW_SO_TIEN_TUONG_TRO_HANG_THANG;
                case "TW_NGUONVON_MACDINH": return MaThamSo.TW_NGUONVON_MACDINH;
                case "TW_CHUYENDIABAN_KIEMTRASODU": return MaThamSo.TW_CHUYENDIABAN_KIEMTRASODU;
                case "TW_TGIAN_LAP_CHUNG_TU": return MaThamSo.TW_TGIAN_LAP_CHUNG_TU;
                case "TW_DIEM_BUTRU_GOCLAI": return MaThamSo.TW_DIEM_BUTRU_GOCLAI;
                case "TW_KHOA_SO_KIEM_TRA_CHUA_DUYET": return MaThamSo.TW_KHOA_SO_KIEM_TRA_CHUA_DUYET;
                case "TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_DUYET": return MaThamSo.TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_DUYET;
                case "TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_GIAI_NGAN": return MaThamSo.TW_KHOA_SO_KIEM_TRA_KUOC_CHUA_GIAI_NGAN;
                case "TW_KIEM_TRA_SO_DU_KHA_DUNG": return MaThamSo.TW_KIEM_TRA_SO_DU_KHA_DUNG;
                case "TW_PLOAI_NO_SO_NGAY": return MaThamSo.TW_PLOAI_NO_SO_NGAY;
                case "TW_PLOAI_NO_TU_DONG": return MaThamSo.TW_PLOAI_NO_TU_DONG;
                case "TW_DANH_SACH_CHUYEN_HOAN_TU_DONG": return MaThamSo.TW_DANH_SACH_CHUYEN_HOAN_TU_DONG;
                case "TW_KIEM_TRA_SO_VONG_VAY": return MaThamSo.TW_KIEM_TRA_SO_VONG_VAY;
                case "TW_KIEM_TRA_SO_TIEN_RUT_GOC": return MaThamSo.TW_KIEM_TRA_SO_TIEN_RUT_GOC;
                case "TW_CACH_TINH_SO_TIEN_MO_SO_TK": return MaThamSo.TW_CACH_TINH_SO_TIEN_MO_SO_TK;
                case "TW_TY_LE_TINH_SO_TIEN_MO_SO_TK": return MaThamSo.TW_TY_LE_TINH_SO_TIEN_MO_SO_TK;
                case "TW_SO_TIEN_MO_SO_TK_TUYET_DOI": return MaThamSo.TW_SO_TIEN_MO_SO_TK_TUYET_DOI;
                case "TW_TKHOAN_DANH_GIA_NGOAI_TE": return MaThamSo.TW_TKHOAN_DANH_GIA_NGOAI_TE;
                case "TW_TKHOAN_THU_NHAP": return MaThamSo.TW_TKHOAN_THU_NHAP;
                case "TW_TKHOAN_CHI_PHI": return MaThamSo.TW_TKHOAN_CHI_PHI;
                case "TW_KSRR_TU_DONG": return MaThamSo.TW_KSRR_TU_DONG;
                case "TW_TDVM_TAM_UNG_TRUOC_GIAI_NGAN": return MaThamSo.TW_TDVM_TAM_UNG_TRUOC_GIAI_NGAN;

                // Tham số làm tròn TDTD
                case "TW_TDTD_PHUONG_THUC_LAM_TRON_GOC": return MaThamSo.TW_TDTD_PHUONG_THUC_LAM_TRON_GOC;
                case "TW_TDTD_PHUONG_THUC_LAM_TRON_LAI": return MaThamSo.TW_TDTD_PHUONG_THUC_LAM_TRON_LAI;
                case "TW_TDTD_MOC_LAM_TRON": return MaThamSo.TW_TDTD_MOC_LAM_TRON;
                case "TW_TDTD_LAM_TRON_LEN": return MaThamSo.TW_TDTD_LAM_TRON_LEN;
                case "TW_TDTD_LAM_TRON_XUONG": return MaThamSo.TW_TDTD_LAM_TRON_XUONG;
                case "TW_TDTD_PHUONG_THUC_CHIA_DEU_GOC": return MaThamSo.TW_TDTD_PHUONG_THUC_CHIA_DEU_GOC;
                case "TW_TDTD_KE_HOACH_LAM_TRON_LAI": return MaThamSo.TW_TDTD_KE_HOACH_LAM_TRON_LAI;
                case "TW_TDTD_KE_HOACH_LAM_TRON_GOC": return MaThamSo.TW_TDTD_KE_HOACH_LAM_TRON_GOC;
                case "TW_TDTD_THU_TU_PHAN_BO_THU_TIEN_KY": return MaThamSo.TW_TDTD_THU_TU_PHAN_BO_THU_TIEN_KY;

                case "TW_CHAY_TU_DONG_CUOI_NGAY": return MaThamSo.TW_CHAY_TU_DONG_CUOI_NGAY;
                case "TW_SINH_BAO_CAO_CUOI_NGAY": return MaThamSo.TW_SINH_BAO_CAO_CUOI_NGAY;
                case "TW_SO_NGAY_THUOC_NHOM_NO": return MaThamSo.TW_SO_NGAY_THUOC_NHOM_NO;
                case "TW_SO_NGAY_THUOC_PART_NO": return MaThamSo.TW_SO_NGAY_THUOC_PART_NO;
                case "TW_TOAN_TU_TINH_LSUAT_QH": return MaThamSo.TW_TOAN_TU_TINH_LSUAT_QH;
                case "TW_SMS_DUYET_TIN_NHAN_DEN": return MaThamSo.TW_SMS_DUYET_TIN_NHAN_DEN;
                case "TW_SMS_DUYET_TIN_NHAN_DI": return MaThamSo.TW_SMS_DUYET_TIN_NHAN_DI;
                case "TW_SMS_SO_NGAY_NHAC_NO": return MaThamSo.TW_SMS_SO_NGAY_NHAC_NO;


                // Tham số riêng cho từng đơn vị
                case "DV_LSUAT_TRAN_CO_KY_HAN": return MaThamSo.DV_LSUAT_TRAN_CO_KY_HAN;
                case "DV_LSUAT_TRAN_KHONG_KY_HAN": return MaThamSo.DV_LSUAT_TRAN_KHONG_KY_HAN;
                case "DV_SO_DU_TINH_LAI_KHI_RUT_GOC": return MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC;
                case "DV_CACH_HACH_TOAN_THOAI_CHI": return MaThamSo.DV_CACH_HACH_TOAN_THOAI_CHI;
                case "DV_PHAN_BO_DOANH_THU": return MaThamSo.DV_PHAN_BO_DOANH_THU;
                case "DV_PHAN_BO_DOANH_THU_GNGAN": return MaThamSo.DV_PHAN_BO_DOANH_THU_GNGAN;
                case "DV_DU_THU_NGOAI_BANG": return MaThamSo.DV_DU_THU_NGOAI_BANG;
                case "DV_DU_THU_DEN_NGAY": return MaThamSo.DV_DU_THU_DEN_NGAY;
                case "DV_NGAY_GIUA_THANG": return MaThamSo.DV_NGAY_GIUA_THANG;
                case "DV_CACH_TINH_LAI_TRUOC_HAN": return MaThamSo.DV_CACH_TINH_LAI_TRUOC_HAN;
                case "DV_LOAI_DVI_KET_CHUYEN": return MaThamSo.DV_LOAI_DVI_KET_CHUYEN;
                case "DV_THOI_GIAN_CTU_GHI_SO": return MaThamSo.DV_THOI_GIAN_CTU_GHI_SO;
                case "DV_LOAI_DTUONG_NHANSU": return MaThamSo.DV_LOAI_DTUONG_NHANSU;
                case "DV_THU_LAI_HACH_TOAN_TUNG": return MaThamSo.DV_THU_LAI_HACH_TOAN_TUNG;
                case "DV_KIEM_TRA_SO_DU_TAT_TOAN_T01": return MaThamSo.DV_KIEM_TRA_SO_DU_TAT_TOAN_T01;
                case "DV_SAN_PHAM_MAC_DINH_T01": return  MaThamSo.DV_SAN_PHAM_MAC_DINH_T01;
                case "DV_SAN_PHAM_MAC_DINH_T02": return MaThamSo.DV_SAN_PHAM_MAC_DINH_T02;
                case "DV_SAN_PHAM_MAC_DINH_HDTDVM": return MaThamSo.DV_SAN_PHAM_MAC_DINH_HDTDVM;
                case "DV_SO_NGAY_TOI_THIEU_TAO_KY_TRA_NO": return MaThamSo.DV_SO_NGAY_TOI_THIEU_TAO_KY_TRA_NO;
                case "DV_NGAY_KET_THUC_NAM_TCHINH": return MaThamSo.DV_NGAY_KET_THUC_NAM_TCHINH;
                case "DV_TIEN_TO_SMS": return MaThamSo.DV_TIEN_TO_SMS;
                case "DV_NGON_NGU_SMS": return MaThamSo.DV_NGON_NGU_SMS;

                case "TW_TY_LE_BHIEM_TU_DUOI_4_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_4_THANG;
                case "TW_TY_LE_BHIEM_TU_DUOI_5_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_5_THANG;
                case "TW_TY_LE_BHIEM_TU_DUOI_7_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_7_THANG;
                case "TW_TY_LE_BHIEM_TU_DUOI_8_THANG": return MaThamSo.TW_TY_LE_BHIEM_TU_DUOI_8_THANG;

                case "TW_CHI_THI_DAO_HAN_NGAY_NGHI": return MaThamSo.TW_CHI_THI_DAO_HAN_NGAY_NGHI;
                case "TW_MUC_LAM_TRON_TD_TINH_LAI_DU_THU": return MaThamSo.TW_MUC_LAM_TRON_TD_TINH_LAI_DU_THU;

                case "DV_LSUAT_SAN_CO_KY_HAN": return MaThamSo.DV_LSUAT_SAN_CO_KY_HAN;
                case "DV_LSUAT_SAN_KHONG_KY_HAN": return MaThamSo.DV_LSUAT_SAN_KHONG_KY_HAN;

                case "DV_KIEM_TRA_HAN_MUC": return MaThamSo.DV_KIEM_TRA_HAN_MUC;

                // Tham số báo cáo cho toàn hệ thống
                default: return MaThamSo.DEFAULT;
            }
        }

        /// <summary>
        /// Loại Tài nguyên khai thác
        /// </summary>
        public enum LoaiTaiNguyen { MENU, FORM, TINH_NANG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LoaiTaiNguyen loaiTaiNguyen)
        {
            switch (loaiTaiNguyen)
            {
                case LoaiTaiNguyen.MENU: return "TN_MENU";
                case LoaiTaiNguyen.FORM: return "TN_FORM";
                case LoaiTaiNguyen.TINH_NANG: return "TN_TNANG";
                default: return "";
            }
        }

        public enum LoaiPhamViDuLieu { CHI_NHANH, PHONG_GIAO_DICH };
        public static string layGiaTri(this LoaiPhamViDuLieu loaiPhamViDuLieu)
        {
            switch (loaiPhamViDuLieu)
            {
                case LoaiPhamViDuLieu.CHI_NHANH: return "CHI_NHANH";
                case LoaiPhamViDuLieu.PHONG_GIAO_DICH: return "PHONG_GIAO_DICH";
                default: return "";
            }
        }
        public static int layIdPhamVi(this LoaiPhamViDuLieu loaiPhamViDuLieu)
        {
            switch (loaiPhamViDuLieu)
            {
                case LoaiPhamViDuLieu.CHI_NHANH: return 1;
                case LoaiPhamViDuLieu.PHONG_GIAO_DICH: return 2;
                default: return 0;
            }
        }
        public static string layMaPhamVi(this LoaiPhamViDuLieu loaiPhamViDuLieu)
        {
            switch (loaiPhamViDuLieu)
            {
                case LoaiPhamViDuLieu.CHI_NHANH: return "CHI_NHANH";
                case LoaiPhamViDuLieu.PHONG_GIAO_DICH: return "PHONG_GIAO_DICH";
                default: return "";
            }
        }

        /// <summary>
        /// Trạng thái sử dụng
        /// </summary>
        public enum TrangThaiBanGhi { SU_DUNG, KHONG_SU_DUNG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TrangThaiBanGhi trangThaiBanGhi)
        {
            switch (trangThaiBanGhi)
            {
                case TrangThaiBanGhi.SU_DUNG: return "SDU";
                case TrangThaiBanGhi.KHONG_SU_DUNG: return "KSD";
                default: return "";
            }
        }

        /// <summary>
        /// Tính chất loại tài khoản
        /// </summary>
        public enum TinhChatLoaiTaiKhoan { NOI_BANG, NGOAI_BANG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TinhChatLoaiTaiKhoan tinhChatLoaiTaiKhoan)
        {
            switch (tinhChatLoaiTaiKhoan)
            {
                case TinhChatLoaiTaiKhoan.NOI_BANG: return "NOI_BANG";
                case TinhChatLoaiTaiKhoan.NGOAI_BANG: return "NGOAI_BANG";
                default: return "";
            }
        }

        /// <summary>
        /// Tính chất loại khách hàng & nội bộ
        /// </summary>
        public enum TinhChatLoaiKhangNBo { NOI_BO, KHACH_HANG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TinhChatLoaiKhangNBo tinhChatLoaiKhangNBo)
        {
            switch (tinhChatLoaiKhangNBo)
            {
                case TinhChatLoaiKhangNBo.NOI_BO: return "NOI_BO";
                case TinhChatLoaiKhangNBo.KHACH_HANG: return "KHACH_HANG";
                default: return "";
            }
        }

        /// <summary>
        /// Tính chất loại thu nhập & chi phó
        /// </summary>
        public enum TinhChatLoaiTNhapCPhi { THU_NHAP, CHI_PHI, KHAC };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TinhChatLoaiTNhapCPhi tinhChatLoaiTNhapCPhi)
        {
            switch (tinhChatLoaiTNhapCPhi)
            {
                case TinhChatLoaiTNhapCPhi.THU_NHAP: return "THU_NHAP";
                case TinhChatLoaiTNhapCPhi.CHI_PHI: return "CHI_PHI";
                case TinhChatLoaiTNhapCPhi.KHAC: return "KHAC";
                default: return "";
            }
        }

        /// <summary>
        /// Trạng thái sử dụng
        /// </summary>
        public enum CoKhong { CO, KHONG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CoKhong coKhong)
        {
            switch (coKhong)
            {
                case CoKhong.CO: return "CO";
                case CoKhong.KHONG: return "KHONG";
                default: return "";
            }
        }

        public static string layNgonNguTuGiaTri(string giaTri, string type)
        {
            if (type.Equals("CO_KHONG"))
            {
                if (giaTri == layGiaTri(CoKhong.CO))
                {
                    return LLanguage.SearchResourceByKey("U.DMUC_GTRI.CO_KHONG.CO");
                }
                else if (giaTri == layGiaTri(CoKhong.KHONG))
                {
                    return LLanguage.SearchResourceByKey("U.DMUC_GTRI.CO_KHONG.KHONG");
                }
            }

            if (type.Equals("TAN_SUAT"))
            {
                if (giaTri == layGiaTri(TAN_SUAT.NGAY))
                {
                    return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TAN_SUAT.NGAY");
                }
                else if (giaTri == layGiaTri(TAN_SUAT.TUAN))
                {
                    return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TAN_SUAT.TUAN");
                }
                else if (giaTri == layGiaTri(TAN_SUAT.QUY))
                {
                    return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TAN_SUAT.QUY");
                }
                else if (giaTri == layGiaTri(TAN_SUAT.THANG))
                {
                    return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TAN_SUAT.THANG");
                }
                else if (giaTri == layGiaTri(TAN_SUAT.NAM))
                {
                    return LLanguage.SearchResourceByKey("U.DMUC_GTRI.TAN_SUAT.NAM");
                }
            }

            return "";
        }

        public static string layMaNgonNguTuGiaTri(string giaTri, string type)
        {
            if (type.Equals("CO_KHONG"))
            {
                if (giaTri == layGiaTri(CoKhong.CO))
                {
                    return "U.DMUC_GTRI.CO_KHONG.CO";
                }
                else if (giaTri == layGiaTri(CoKhong.KHONG))
                {
                    return "U.DMUC_GTRI.CO_KHONG.KHONG";
                }
            }

            if (type.Equals("TAN_SUAT"))
            {
                if (giaTri == layGiaTri(TAN_SUAT.NGAY))
                {
                    return "U.DMUC_GTRI.TAN_SUAT.NGAY";
                }
                else if (giaTri == layGiaTri(TAN_SUAT.TUAN))
                {
                    return "U.DMUC_GTRI.TAN_SUAT.TUAN";
                }
                else if (giaTri == layGiaTri(TAN_SUAT.QUY))
                {
                    return "U.DMUC_GTRI.TAN_SUAT.QUY";
                }
                else if (giaTri == layGiaTri(TAN_SUAT.THANG))
                {
                    return "U.DMUC_GTRI.TAN_SUAT.THANG";
                }
                else if (giaTri == layGiaTri(TAN_SUAT.NAM))
                {
                    return "U.DMUC_GTRI.TAN_SUAT.NAM";
                }
            }

            return "";
        }

        /// <summary>
        /// Nhóm sản phẩm tìm kiếm
        ///"T01":Tiết kiệm quy định;
        ///"T02":Tiết kiệm tự nguyện không kỳ hạn;
        ///"T03":Tiết kiệm tự nguyện trả lãi định kỳ;
        ///"T04":Tiết kiệm tự nguyện có kỳ hạn trả lãi sau;
        ///"T05":Tiết kiệm tự nguyện có kỳ hạn trả lãi trước;
        ///"T06":Tiết kiệm tự nguyện có kỳ hạn gửi góp;
        ///"T07":Tiền gửi có kỳ hạn;
        ///"T08": //Tiền gửi thanh toán
        /// </summary>
        public enum NHOM_SPHAM_TKIEM
        {
            T01,
            T02,
            T03,
            T04,
            T05,
            T06,
            T07,
            T08
        };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this NHOM_SPHAM_TKIEM NHOM_SPHAM_TKIEM)
        {
            switch (NHOM_SPHAM_TKIEM)
            {
                case NHOM_SPHAM_TKIEM.T01: return "T01"; //Tiết kiệm quy định
                case NHOM_SPHAM_TKIEM.T02: return "T02"; //Tiết kiệm tự nguyện không kỳ hạn
                case NHOM_SPHAM_TKIEM.T03: return "T03"; //Tiết kiệm tự nguyện trả lãi định kỳ
                case NHOM_SPHAM_TKIEM.T04: return "T04"; //Tiết kiệm tự nguyện có kỳ hạn trả lãi sau
                case NHOM_SPHAM_TKIEM.T05: return "T05"; //Tiết kiệm tự nguyện có kỳ hạn trả lãi trước
                case NHOM_SPHAM_TKIEM.T06: return "T06"; //Tiết kiệm tự nguyện có kỳ hạn gửi góp
                case NHOM_SPHAM_TKIEM.T07: return "T07"; //Tiền gửi có kỳ hạn
                case NHOM_SPHAM_TKIEM.T08: return "T08"; //Tiền gửi thanh toán
                default: return "";

            }
        }

        /// <summary>
        /// Phương pháp tính lãi gửi góp
        /// </summary>
        public enum PHUONG_THUC_TRA_LAI { TRA_LAI_TRUOC, TRA_LAI_SAU, DKY_DAU, DKY_CUOI, LAI_NHAP_GOC };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PHUONG_THUC_TRA_LAI PHUONG_THUC_TRA_LAI)
        {
            switch (PHUONG_THUC_TRA_LAI)
            {
                case PHUONG_THUC_TRA_LAI.TRA_LAI_TRUOC: return "TRA_LAI_TRUOC";
                case PHUONG_THUC_TRA_LAI.TRA_LAI_SAU: return "TRA_LAI_SAU";
                case PHUONG_THUC_TRA_LAI.DKY_DAU: return "DKY_DAU";
                case PHUONG_THUC_TRA_LAI.DKY_CUOI: return "DKY_CUOI";
                case PHUONG_THUC_TRA_LAI.LAI_NHAP_GOC: return "LAI_NHAP_GOC";
                default: return "";
            }
        }

        /// <summary>
        /// Tần suất thời gian
        /// </summary>
        public enum TAN_SUAT { NGAY, TUAN, QUY, THANG, NAM, NA };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TAN_SUAT TAN_SUAT)
        {
            switch (TAN_SUAT)
            {
                case TAN_SUAT.NGAY: return "NGAY";
                case TAN_SUAT.TUAN: return "TUAN";
                case TAN_SUAT.QUY: return "QUY";
                case TAN_SUAT.THANG: return "THANG";
                case TAN_SUAT.NAM: return "NAM";
                case TAN_SUAT.NA: return "NA";
                default: return "";
            }
        }

        /// <summary>
        /// Đơn vị tính thời gian
        /// </summary>
        public enum DON_VI_TINH_THOI_GIAN { NGAY, NGAY10, NGAY15, NGAY20, TUAN, THANG, QUY, NAM };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this DON_VI_TINH_THOI_GIAN donViTinhThoiGian)
        {
            switch (donViTinhThoiGian)
            {
                case DON_VI_TINH_THOI_GIAN.NGAY: return "NGAY";
                case DON_VI_TINH_THOI_GIAN.NGAY10: return "NGAY10";
                case DON_VI_TINH_THOI_GIAN.NGAY15: return "NGAY15";
                case DON_VI_TINH_THOI_GIAN.NGAY20: return "NGAY20";
                case DON_VI_TINH_THOI_GIAN.TUAN: return "TUAN";
                case DON_VI_TINH_THOI_GIAN.QUY: return "QUY";
                case DON_VI_TINH_THOI_GIAN.THANG: return "THANG";
                case DON_VI_TINH_THOI_GIAN.NAM: return "NAM";
                default: return "";
            }
        }

        /// <summary>
        /// Phương thức tính lãi
        /// </summary>
        public enum PHUONG_THUC_TINH_LAI { DNO_BDAU, DNO_GDAN };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PHUONG_THUC_TINH_LAI PHUONG_THUC_TINH_LAI)
        {
            switch (PHUONG_THUC_TINH_LAI)
            {
                case PHUONG_THUC_TINH_LAI.DNO_BDAU: return "DNO_BDAU";
                case PHUONG_THUC_TINH_LAI.DNO_GDAN: return "DNO_GDAN";
                default: return "";
            }
        }

        /// <summary>
        /// Phương pháp tính lãi gửi góp
        /// </summary>
        public enum PPHAP_TLAI_GGOP { LAI_DON, LAI_KEP };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PPHAP_TLAI_GGOP PPHAP_TLAI_GGOP)
        {
            switch (PPHAP_TLAI_GGOP)
            {
                case PPHAP_TLAI_GGOP.LAI_DON: return "LAI_DON";
                case PPHAP_TLAI_GGOP.LAI_KEP: return "LAI_KEP";
                default: return "";
            }
        }

        /// <summary>
        /// Phương pháp tính lãi gửi góp
        /// </summary>
        public enum PPHAP_TINH_RGOC { TUONGDOI, TUYETDOI };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PPHAP_TINH_RGOC PPHAP_TINH_RGOC)
        {
            switch (PPHAP_TINH_RGOC)
            {
                case PPHAP_TINH_RGOC.TUONGDOI: return "TUONG_DOI";
                case PPHAP_TINH_RGOC.TUYETDOI: return "TUYET_DOI";
                default: return "";
            }
        }

        /// <summary>
        /// Phương pháp tính lãi gửi góp
        /// </summary>
        public enum PPHAP_TPHI_TGUI_TTOAN { SDU_HIEN_TAI, SDU_BQUAN_THANG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PPHAP_TPHI_TGUI_TTOAN PPHAP_TPHI_TGUI_TTOAN)
        {
            switch (PPHAP_TPHI_TGUI_TTOAN)
            {
                case PPHAP_TPHI_TGUI_TTOAN.SDU_HIEN_TAI: return "SDU_HIEN_TAI";
                case PPHAP_TPHI_TGUI_TTOAN.SDU_BQUAN_THANG: return "SDU_BQUAN_THANG";
                default: return "";
            }
        }

        /// <summary>
        /// Phương pháp tính lãi gửi góp
        /// </summary>
        public enum HINH_THUC_GIAO_DICH { TIEN_MAT, CHUYEN_KHOAN, TMAT_CKHOAN };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this HINH_THUC_GIAO_DICH HINH_THUC_GIAO_DICH)
        {
            switch (HINH_THUC_GIAO_DICH)
            {
                case HINH_THUC_GIAO_DICH.TIEN_MAT: return "TIEN_MAT";
                case HINH_THUC_GIAO_DICH.CHUYEN_KHOAN: return "CHUYEN_KHOAN";
                case HINH_THUC_GIAO_DICH.TMAT_CKHOAN: return "TMAT_CKHOAN";
                default: return "";
            }
        }

        /// <summary>
        /// Chỉ thị đáo hạn
        /// </summary>
        public enum CHI_THI_DAO_HAN
        {
            LNG_LSQV_KHC,
            SPQV_LSM_KHM,
            GQV_LSC_TKCT,
            GQV_LSM_TKCT,
            GOC_LAI_TKCT,
            GQV_KHC_LSM_TKCT,
            LNG_KHC_LSM
        }
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CHI_THI_DAO_HAN CHI_THI_DAO_HAN)
        {
            switch (CHI_THI_DAO_HAN)
            {
                case CHI_THI_DAO_HAN.LNG_LSQV_KHC: return "LNG_LSQV_KHC";
                case CHI_THI_DAO_HAN.SPQV_LSM_KHM: return "SPQV_LSM_KHM";
                case CHI_THI_DAO_HAN.GQV_LSC_TKCT: return "GQV_LSC_TKCT";
                case CHI_THI_DAO_HAN.GQV_LSM_TKCT: return "GQV_LSM_TKCT";
                case CHI_THI_DAO_HAN.GOC_LAI_TKCT: return "GOC_LAI_TKCT";
                case CHI_THI_DAO_HAN.GQV_KHC_LSM_TKCT: return "GQV_KHC_LSM_TKCT";
                case CHI_THI_DAO_HAN.LNG_KHC_LSM: return "LNG_KHC_LSM";
                default: return "";
            }
        }

        /// <summary>
        /// Phương pháp tính lãi suất
        /// </summary>
        public enum PPHAP_TINH_LSUAT { DTH, BTH };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PPHAP_TINH_LSUAT PPHAP_TINH_LSUAT)
        {
            switch (PPHAP_TINH_LSUAT)
            {
                case PPHAP_TINH_LSUAT.DTH: return "DTH";
                case PPHAP_TINH_LSUAT.BTH: return "BTH";
                default: return "";
            }
        }

        /// <summary>
        /// Hình thức bậc thang
        /// </summary>
        public enum HTHUC_BTHANG { KY_HAN, SO_TIEN, KHAN_STIEN };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this HTHUC_BTHANG HTHUC_BTHANG)
        {
            switch (HTHUC_BTHANG)
            {
                case HTHUC_BTHANG.KY_HAN: return "KY_HAN";
                case HTHUC_BTHANG.SO_TIEN: return "SO_TIEN";
                case HTHUC_BTHANG.KHAN_STIEN: return "KHAN_STIEN";
                default: return "";
            }
        }

        /// <summary>
        /// Hình thức bậc thang
        /// </summary>
        public enum HTHUC_BTHANG_PHI { SO_TIEN, TY_LE, STIEN_TLE };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this HTHUC_BTHANG_PHI HTHUC_BTHANG_PHI)
        {
            switch (HTHUC_BTHANG_PHI)
            {
                case HTHUC_BTHANG_PHI.SO_TIEN: return "SO_TIEN";
                case HTHUC_BTHANG_PHI.TY_LE: return "TY_LE";
                case HTHUC_BTHANG_PHI.STIEN_TLE: return "STIEN_TLE";
                default: return "";
            }
        }

        /// <summary>
        /// Hình thức bậc thang
        /// </summary>
        public enum LOAI_PHI { PHI, HOA_HONG };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_PHI LOAI_PHI)
        {
            switch (LOAI_PHI)
            {
                case LOAI_PHI.PHI: return "PHI";
                case LOAI_PHI.HOA_HONG: return "HOA_HONG";
                default: return "";
            }
        }

        /// <summary>
        /// Phương pháp điều chỉnh 
        /// </summary>
        public enum PPHAP_DIEU_CHINH
        {
            DAO_CHIEU,
            GHI_AM
        };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="phuongPhapDieuChinh">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PPHAP_DIEU_CHINH phuongPhapDieuChinh)
        {
            switch (phuongPhapDieuChinh)
            {
                case PPHAP_DIEU_CHINH.DAO_CHIEU: return "DAO_CHIEU";
                case PPHAP_DIEU_CHINH.GHI_AM: return "GHI_AM";
                default: return "";
            }
        }

        /// <summary>
        /// Cấu trúc tài khoản
        /// </summary>
        public enum CAU_TRUC_TAI_KHOAN
        {
            TAI_KHOAN_NOI_BO,
            TAI_KHOAN_CHI_TIET
        };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="cauTrucTaiKhoan">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CAU_TRUC_TAI_KHOAN cauTrucTaiKhoan)
        {
            switch (cauTrucTaiKhoan)
            {
                case CAU_TRUC_TAI_KHOAN.TAI_KHOAN_NOI_BO: return "TAI_KHOAN_NOI_BO";
                case CAU_TRUC_TAI_KHOAN.TAI_KHOAN_CHI_TIET: return "TAI_KHOAN_CHI_TIET";
                default: return "";
            }
        }

        /// <summary>
        /// Loại phát sinh
        /// </summary>
        public enum LOAI_PHAT_SINH
        {
            GNTM,
            GNCK,
            GCTM,
            GCCK
        };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiPhatSinh">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_PHAT_SINH loaiPhatSinh)
        {
            switch (loaiPhatSinh)
            {
                case LOAI_PHAT_SINH.GNTM: return "GNTM"; //Ghi nợ tiền mặt
                case LOAI_PHAT_SINH.GNCK: return "GNCK"; //Ghi nợ chuyển khoản
                case LOAI_PHAT_SINH.GCTM: return "GCTM"; //Ghi có tiền mặt
                case LOAI_PHAT_SINH.GCCK: return "GCCK"; //Ghi có chuyển khoản
                default: return "";
            }
        }

        /// <summary>
        /// Loại hạn mức
        /// </summary>
        public enum LOAI_HMUC { HMTD, HMGD, HMTU, HMKH };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_HMUC LOAI_HMUC)
        {
            switch (LOAI_HMUC)
            {
                case LOAI_HMUC.HMTD: return "HMTD";
                case LOAI_HMUC.HMGD: return "HMGD";
                case LOAI_HMUC.HMTU: return "HMTU";
                case LOAI_HMUC.HMKH: return "HMKH";
                default: return "";
            }
        }

        /// <summary>
        /// Loại đối tượng áp dụng hạn mức
        /// </summary>
        public enum LOAI_DTUONG_HMUC { HDTD, KUOC };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_DTUONG_HMUC LOAI_DTUONG_HMUC)
        {
            switch (LOAI_DTUONG_HMUC)
            {
                case LOAI_DTUONG_HMUC.HDTD: return "HDTD";
                case LOAI_DTUONG_HMUC.KUOC: return "KUOC";
                default: return "";
            }
        }

        /// <summary>
        /// Nhóm nợ
        /// </summary>
        public enum NHOM_NO { NHOM1, NHOM2, NHOM3, NHOM4, NHOM5, NGOAI_BANG };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this NHOM_NO NHOM_NO)
        {
            switch (NHOM_NO)
            {
                case NHOM_NO.NHOM1: return "NHOM1";
                case NHOM_NO.NHOM2: return "NHOM2";
                case NHOM_NO.NHOM3: return "NHOM3";
                case NHOM_NO.NHOM4: return "NHOM4";
                case NHOM_NO.NHOM5: return "NHOM5";
                case NHOM_NO.NGOAI_BANG: return "NGOAI_BANG";
                default: return "";
            }
        }

        /// <summary>
        /// Trạng thái giao dịch ngày làm việc
        /// </summary>
        public enum TRANG_THAI_GIAO_DICH
        {
            GIAO_DICH, TAM_DUNG, NGUNG_GD
        };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TRANG_THAI_GIAO_DICH TRANG_THAI_GIAO_DICH)
        {
            switch (TRANG_THAI_GIAO_DICH)
            {
                case TRANG_THAI_GIAO_DICH.GIAO_DICH: return "GIAO_DICH";
                case TRANG_THAI_GIAO_DICH.TAM_DUNG: return "TAM_DUNG";
                case TRANG_THAI_GIAO_DICH.NGUNG_GD: return "NGUNG_GD";
                default: return "";
            }
        }


        /// <summary>
        /// Nghiệp vụ khi khóa sỗ cuối ngày
        /// </summary>
        public enum NGHIEP_VU_CUOI_NGAY
        {
            TAO_NGAY_LVIEC, 
            TAM_DUNG_NGAY_LVIEC, 
            KIEM_TRA_GD, 
            TONG_HOP, 
            DUNG_NGAY_LVIEC, 
            NGHIEP_VU_MO_SO, 
            NGHIEP_VU_KHOA_SO, 
            NGHIEP_VU_CUOI_NGAY_TW,
            DEFAULT
        };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this NGHIEP_VU_CUOI_NGAY NGHIEP_VU_CUOI_NGAY)
        {
            switch (NGHIEP_VU_CUOI_NGAY)
            {
                case NGHIEP_VU_CUOI_NGAY.TAO_NGAY_LVIEC: return "TAO_NGAY_LVIEC";
                case NGHIEP_VU_CUOI_NGAY.TAM_DUNG_NGAY_LVIEC: return "TAM_DUNG_NGAY_LVIEC";
                case NGHIEP_VU_CUOI_NGAY.KIEM_TRA_GD: return "KIEM_TRA_GD";
                case NGHIEP_VU_CUOI_NGAY.TONG_HOP: return "TONG_HOP";
                case NGHIEP_VU_CUOI_NGAY.DUNG_NGAY_LVIEC: return "DUNG_NGAY_LVIEC";
                case NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_MO_SO: return "NGHIEP_VU_MO_SO";
                case NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_KHOA_SO: return "NGHIEP_VU_KHOA_SO";
                case NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_CUOI_NGAY_TW: return "NGHIEP_VU_CUOI_NGAY_TW";
                default: return "";
            }
        }

        public static NGHIEP_VU_CUOI_NGAY layEnumNghiepVuCuoiNgay(string nghiepVu)
        {
            switch (nghiepVu)
            {
                case "TAO_NGAY_LVIEC": return NGHIEP_VU_CUOI_NGAY.TAO_NGAY_LVIEC;
                case "TAM_DUNG_NGAY_LVIEC": return NGHIEP_VU_CUOI_NGAY.TAM_DUNG_NGAY_LVIEC;
                case "KIEM_TRA_GD": return NGHIEP_VU_CUOI_NGAY.KIEM_TRA_GD;
                case "TONG_HOP": return NGHIEP_VU_CUOI_NGAY.TONG_HOP;
                case "DUNG_NGAY_LVIEC": return NGHIEP_VU_CUOI_NGAY.DUNG_NGAY_LVIEC;
                case "NGHIEP_VU_KHOA_SO": return NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_KHOA_SO;
                case "NGHIEP_VU_CUOI_NGAY_TW": return NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_CUOI_NGAY_TW;
                default: return NGHIEP_VU_CUOI_NGAY.DEFAULT;
            }
        }

        /// <summary>
        /// Nghiệp vụ khi khóa sỗ cuối ngày
        /// </summary>
        public enum LOAI_CUOI_NGAY
        {
            MO_SO, KHOA_SO
        };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_CUOI_NGAY LOAI_CUOI_NGAY)
        {
            switch (LOAI_CUOI_NGAY)
            {
                case LOAI_CUOI_NGAY.MO_SO: return "MO_SO";
                case LOAI_CUOI_NGAY.KHOA_SO: return "KHOA_SO";
                default: return "";
            }
        }

        public enum LOAI_CHUNG_TU
        {
            PHIEU_THU, // Phiếu thu
            PHIEU_CHI, // Phiếu chi
            CHUYEN_KHOAN, // Phiếu chuyển khoản
            PHIEU_KETOAN, // Phiếu kế toán
            XUAT_NGOAI_BANG, // Xuất ngoại bảng
            NHAP_NGOAI_BANG,  //Nhập ngoại bảng
            UY_NHIEM_CHI,
            DEFAULT
        }

        public static string layMaPhanHeTheoLoaiChungTu(string maLoaiChungTu)
        {
            switch (maLoaiChungTu)
            {
                case "PHIEU_THU": return "PTHU";
                case "PHIEU_CHI": return "PCHI";
                case "PHIEU_KETOAN": return "PKET";
                case "XUAT_NGOAI_BANG": return "XNB";
                case "NHAP_NGOAI_BANG": return "NNB";
                case "UY_NHIEM_CHI": return "UNCHI";
                default: return "";
            }
        }

        public static LOAI_CHUNG_TU layPhanHeTheoLoaiChungTu(string maLoaiChungTu)
        {
            switch (maLoaiChungTu)
            {
                case "PTH": return LOAI_CHUNG_TU.PHIEU_THU;
                case "PCH": return LOAI_CHUNG_TU.PHIEU_CHI;
                case "PKT": return LOAI_CHUNG_TU.PHIEU_KETOAN;
                case "XNB": return LOAI_CHUNG_TU.XUAT_NGOAI_BANG;
                case "NNB": return LOAI_CHUNG_TU.NHAP_NGOAI_BANG;
                case "UNCHI": return LOAI_CHUNG_TU.UY_NHIEM_CHI;
                default: return LOAI_CHUNG_TU.DEFAULT;
            }
        }

        public static string layGiaTri(this LOAI_CHUNG_TU LOAI_CHUNG_TU)
        {
            switch (LOAI_CHUNG_TU)
            {
                case LOAI_CHUNG_TU.PHIEU_THU: return "PHIEU_THU";
                case LOAI_CHUNG_TU.PHIEU_CHI: return "PHIEU_CHI";
                case LOAI_CHUNG_TU.CHUYEN_KHOAN: return "CHUYEN_KHOAN";
                case LOAI_CHUNG_TU.PHIEU_KETOAN: return "PHIEU_KETOAN";
                case LOAI_CHUNG_TU.XUAT_NGOAI_BANG: return "XUAT_NGOAI_BANG";
                case LOAI_CHUNG_TU.NHAP_NGOAI_BANG: return "NHAP_NGOAI_BANG";
                case LOAI_CHUNG_TU.UY_NHIEM_CHI: return "UY_NHIEM_CHI";
                default: return "";
            }
        }

        public static string layLoaiChungTu(this LOAI_CHUNG_TU LOAI_CHUNG_TU)
        {
            switch (LOAI_CHUNG_TU)
            {
                case LOAI_CHUNG_TU.PHIEU_THU: return "PTH";
                case LOAI_CHUNG_TU.PHIEU_CHI: return "PCH";
                case LOAI_CHUNG_TU.CHUYEN_KHOAN: return "CKH";
                case LOAI_CHUNG_TU.PHIEU_KETOAN: return "PKT";
                case LOAI_CHUNG_TU.XUAT_NGOAI_BANG: return "XNB";
                case LOAI_CHUNG_TU.NHAP_NGOAI_BANG: return "NNB";
                case LOAI_CHUNG_TU.UY_NHIEM_CHI: return "PKT";
                default: return "";
            }
        }

        public static LOAI_CHUNG_TU layLoaiChungTu(string loaiChungTu)
        {
            switch (loaiChungTu)
            {
                case "PHIEU_THU": return LOAI_CHUNG_TU.PHIEU_THU;
                case "PHIEU_CHI": return LOAI_CHUNG_TU.PHIEU_CHI;
                case "CHUYEN_KHOAN": return LOAI_CHUNG_TU.CHUYEN_KHOAN;
                case "PHIEU_KETOAN": return LOAI_CHUNG_TU.PHIEU_KETOAN;
                case "XUAT_NGOAI_BANG": return LOAI_CHUNG_TU.XUAT_NGOAI_BANG;
                case "NHAP_NGOAI_BANG": return LOAI_CHUNG_TU.NHAP_NGOAI_BANG;
                case "UY_NHIEM_CHI": return LOAI_CHUNG_TU.UY_NHIEM_CHI;
                default: return LOAI_CHUNG_TU.DEFAULT;
            }
        }

        public static string layMaLoaiChungTuMacDinh(this LOAI_CHUNG_TU LOAI_CHUNG_TU)
        {
            switch (LOAI_CHUNG_TU)
            {
                case LOAI_CHUNG_TU.PHIEU_THU: return "KT01";
                case LOAI_CHUNG_TU.PHIEU_CHI: return "KT02";
                case LOAI_CHUNG_TU.CHUYEN_KHOAN: return "";
                case LOAI_CHUNG_TU.PHIEU_KETOAN: return "KT04";
                case LOAI_CHUNG_TU.XUAT_NGOAI_BANG: return "KT03";
                case LOAI_CHUNG_TU.NHAP_NGOAI_BANG: return "KT03";
                case LOAI_CHUNG_TU.UY_NHIEM_CHI: return "KT07";
                default: return "";
            }
        }

        /// <summary>
        /// Thời hạn cho vay
        /// </summary>
        public enum THOI_HAN_CHO_VAY { NGAN_HAN, TRUNG_HAN, DAI_HAN };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this THOI_HAN_CHO_VAY thoiHanChoVay)
        {
            switch (thoiHanChoVay)
            {
                case THOI_HAN_CHO_VAY.NGAN_HAN: return "NGAN_HAN";
                case THOI_HAN_CHO_VAY.TRUNG_HAN: return "TRUNG_HAN";
                case THOI_HAN_CHO_VAY.DAI_HAN: return "DAI_HAN";
                default: return "";
            }
        }

        /// <summary>
        /// Hình thức trả lãi
        /// </summary>
        public enum HINH_THUC_TRA_LAI { DAU_KY, DINH_KY, CUOI_KY };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this HINH_THUC_TRA_LAI hinhThucTraLai)
        {
            switch (hinhThucTraLai)
            {
                case HINH_THUC_TRA_LAI.DAU_KY: return "DKY"; // "DAU_KY";
                case HINH_THUC_TRA_LAI.DINH_KY: return "DHK"; //DINH_KY";
                case HINH_THUC_TRA_LAI.CUOI_KY: return "CKY"; //"CUOI_KY";
                default: return "";
            }
        }

        /// <summary>
        /// Hình thức trả lãi
        /// </summary>
        public enum HINH_THUC_TRA_GOC { TRA_SAU, DINH_KY };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this HINH_THUC_TRA_GOC hinhThucTraGoc)
        {
            switch (hinhThucTraGoc)
            {
                case HINH_THUC_TRA_GOC.TRA_SAU: return "CKY"; // "DAU_KY";
                case HINH_THUC_TRA_GOC.DINH_KY: return "DHK"; //DINH_KY";
                default: return "";
            }
        }

        /// <summary>
        /// Hình thức thanh toán
        /// </summary>
        public enum HINH_THUC_THANH_TOAN { DAU_KY, CUOI_KY, DINH_KY };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this HINH_THUC_THANH_TOAN hinhThucTToan)
        {
            switch (hinhThucTToan)
            {
                case HINH_THUC_THANH_TOAN.DAU_KY: return "DKY";
                case HINH_THUC_THANH_TOAN.CUOI_KY: return "CKY";
                case HINH_THUC_THANH_TOAN.DINH_KY: return "DHK";
                default: return "";
            }
        }

        /// <summary>
        /// Phương pháp gửi tiền mỗi kỳ
        /// </summary>
        public enum PPHAP_TINH_GTIEN { TUONGDOI, TUYETDOI };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this PPHAP_TINH_GTIEN pPhapTinhGTien)
        {
            switch (pPhapTinhGTien)
            {
                case PPHAP_TINH_GTIEN.TUONGDOI: return "TUONG_DOI";
                case PPHAP_TINH_GTIEN.TUYETDOI: return "TUYET_DOI";
                default: return "";
            }
        }

        /// <summary>
        /// Cách thức gửi tiền mỗi kỳ
        /// </summary>
        public enum CACH_THUC_GTIEN { THEOKHOANG, THEOTYLE };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CACH_THUC_GTIEN cThucGTien)
        {
            switch (cThucGTien)
            {
                case CACH_THUC_GTIEN.THEOKHOANG: return "THEOKHOANG";
                case CACH_THUC_GTIEN.THEOTYLE: return "THEOTYLE";
                default: return "";
            }
        }

        /// <summary>
        /// Cơ sở tính theo tỷ lệ gửi tiền
        /// </summary>
        public enum CSO_TINH_TLE_GTIEN { TONG_DNO_KUOC, DNO_KUOC_GNHAT, DNO_KUOC_DTIEN };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CSO_TINH_TLE_GTIEN cSoTinhTLeGTien)
        {
            switch (cSoTinhTLeGTien)
            {
                case CSO_TINH_TLE_GTIEN.TONG_DNO_KUOC: return "TONG_DNO_KUOC";
                case CSO_TINH_TLE_GTIEN.DNO_KUOC_GNHAT: return "DNO_KUOC_GNHAT";
                case CSO_TINH_TLE_GTIEN.DNO_KUOC_DTIEN: return "DNO_KUOC_DTIEN";
                default: return "";
            }
        }

        /// <summary>
        /// Loại thông tin chi tiết sản phẩm
        /// </summary>
        public enum LOAI_TTIN { TGUI_1KY, BIEU_PHI };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_TTIN loaiTTin)
        {
            switch (loaiTTin)
            {
                case LOAI_TTIN.TGUI_1KY: return "TGUI_1KY";
                case LOAI_TTIN.BIEU_PHI: return "BIEU_PHI";
                default: return "";
            }
        }

        /// <summary>
        /// Loại thông tin kế hoạch trả nợ
        /// </summary>
        public enum LOAI_TTIN_KHOACH_TRNO { PHAT_VON, KHOACH_TRA_NO };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_TTIN_KHOACH_TRNO loaiTTin)
        {
            switch (loaiTTin)
            {
                case LOAI_TTIN_KHOACH_TRNO.PHAT_VON: return "PHAT_VON";
                case LOAI_TTIN_KHOACH_TRNO.KHOACH_TRA_NO: return "KHOACH_TRA_NO";
                default: return "";
            }
        }

        /// <summary>
        /// Tính chất biến động của lãi suất
        /// </summary>
        public enum LOAI_LAI_SUAT { CO_DINH, THA_NOI };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_LAI_SUAT tinhChatLaiSuat)
        {
            switch (tinhChatLaiSuat)
            {
                case LOAI_LAI_SUAT.CO_DINH: return "CO_DINH";
                case LOAI_LAI_SUAT.THA_NOI: return "THA_NOI";
                default: return "";
            }
        }

        /// <summary>
        /// Tính chất biểu phí
        /// </summary>
        public enum TCHAT_BPHI { DTH, BTH };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TCHAT_BPHI tinhChatBieuPhi)
        {
            switch (tinhChatBieuPhi)
            {
                case TCHAT_BPHI.DTH: return "DTH";
                case TCHAT_BPHI.BTH: return "BTH";
                default: return "";
            }
        }

        /// <summary>
        /// Loại sổ phụ 
        /// </summary>
        public enum LOAI_SO_PHU { SO_PHU_TGUI_CO_KY_HAN, SO_PHU_TGUI_KHONG_KY_HAN };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this LOAI_SO_PHU LOAI_SO_PHU)
        {
            switch (LOAI_SO_PHU)
            {
                case LOAI_SO_PHU.SO_PHU_TGUI_CO_KY_HAN: return "SO_PHU_TGUI_CO_KY_HAN";
                case LOAI_SO_PHU.SO_PHU_TGUI_KHONG_KY_HAN: return "SO_PHU_TGUI_KHONG_KY_HAN";
                default: return "";
            }
        }

        /// <summary>
        /// Đơn vị tính kỳ hạn
        /// </summary>
        public enum KY_HAN_DVI_TINH { NGAY, TUAN, THANG, QUY, NAM };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="kyHanDVT">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this KY_HAN_DVI_TINH kyHanDVT)
        {
            switch (kyHanDVT)
            {
                case KY_HAN_DVI_TINH.NGAY: return "NGAY";
                case KY_HAN_DVI_TINH.TUAN: return "TUAN";
                case KY_HAN_DVI_TINH.THANG: return "THANG";
                case KY_HAN_DVI_TINH.QUY: return "QUY";
                case KY_HAN_DVI_TINH.NAM: return "NAM";
                default: return "";
            }
        }

        /// <summary>
        /// Căn cứ xét nợ quá hạn
        /// </summary>
        public enum CAN_CU_XET_QUA_HAN { KY_TRA_NO, NGAY_DAO_HAN_KHE_UOC };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CAN_CU_XET_QUA_HAN canCuXetQuaHan)
        {
            switch (canCuXetQuaHan)
            {
                case CAN_CU_XET_QUA_HAN.KY_TRA_NO: return "KY_TRA_NO";
                case CAN_CU_XET_QUA_HAN.NGAY_DAO_HAN_KHE_UOC: return "NGAY_DAO_HAN_KHE_UOC";
                default: return "";
            }
        }

        /// <summary>
        /// Căn cứ xét nợ quá hạn
        /// </summary>
        public enum MA_KY_HIEU
        {
            TIENMAT,
            DUPH01,
            DUPH02
        };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this MA_KY_HIEU maKyHieu)
        {
            switch (maKyHieu)
            {
                case MA_KY_HIEU.TIENMAT: return "TIENMAT";
                case MA_KY_HIEU.DUPH01: return "DUPH01";
                case MA_KY_HIEU.DUPH02: return "DUPH02";
                default: return "";
            }
        }

        /// <summary>
        /// Cách quản lý vòng vay vốn
        /// </summary>
        public enum CACH_QUANLY_VVVON { GAN_VAO_SP, KHONG_GAN_VAO_SP };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this CACH_QUANLY_VVVON cachQLyVVVon)
        {
            switch (cachQLyVVVon)
            {
                case CACH_QUANLY_VVVON.GAN_VAO_SP: return "GAN_VAO_SP";
                case CACH_QUANLY_VVVON.KHONG_GAN_VAO_SP: return "KHONG_GAN_VAO_SP";
                default: return "";
            }
        }

        /// <summary>
        /// Toán tử xác định số vòng vay
        /// </summary>
        public enum TOAN_TU { BANG, LON_HON_HOAC_BANG };

        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="thoiHanChoVay">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this TOAN_TU toanTu)
        {
            switch (toanTu)
            {
                case TOAN_TU.BANG: return "=";
                case TOAN_TU.LON_HON_HOAC_BANG: return ">=";
                default: return "";
            }
        }

        public enum PHUONG_PHAP_DIEU_CHINH
        {
            BTOAN_DCHIEU,
            BTOAN_AM
        }

        public static string layGiaTri(this PHUONG_PHAP_DIEU_CHINH phuongphap)
        {
            switch (phuongphap)
            {
                case PHUONG_PHAP_DIEU_CHINH.BTOAN_DCHIEU: return "BTOAN_DCHIEU";
                case PHUONG_PHAP_DIEU_CHINH.BTOAN_AM: return "BTOAN_AM";
                default: return "";
            }
        }


        //Trạng thái tất toán
        public enum TRANG_THAI_TAT_TOAN
        {
            DA_TAT_TOAN,
            CHUA_TAT_TOAN,
            XUAT_NGOAI_BANG,
            PHONG_TOA_TKHOAN,
            XY_LY_NO,
            CHUA_GIAI_NGAN,
            KHONG_GIAI_NGAN,
            DOI_GIAI_NGAN
        }

        public static string layGiaTri(this TRANG_THAI_TAT_TOAN trangthai)
        {
            switch (trangthai)
            {
                case TRANG_THAI_TAT_TOAN.DA_TAT_TOAN: return "DTT";
                case TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN: return "CTT";
                case TRANG_THAI_TAT_TOAN.XUAT_NGOAI_BANG: return "XNB";
                case TRANG_THAI_TAT_TOAN.PHONG_TOA_TKHOAN: return "PTTK";
                case TRANG_THAI_TAT_TOAN.XY_LY_NO: return "XLN";
                case TRANG_THAI_TAT_TOAN.CHUA_GIAI_NGAN: return "CGN";
                case TRANG_THAI_TAT_TOAN.KHONG_GIAI_NGAN: return "KGN";
                case TRANG_THAI_TAT_TOAN.DOI_GIAI_NGAN: return "DGN";
                default: return "";
            }
        }

        //Tính chất kỳ hạn
        public enum TINH_CHAT_KY_HAN
        {
            CO_KY_HAN_TU_NGUYEN,
            KHONG_KY_HAN_TU_NGUYEN,
            KHONG_KY_HAN_QUY_DINH_1,
            KHONG_KY_HAN_TRON_THANG
        }

        public static string layGiaTri(this TINH_CHAT_KY_HAN tinhChat)
        {
            switch (tinhChat)
            {
                case TINH_CHAT_KY_HAN.CO_KY_HAN_TU_NGUYEN: return "CKHTN";
                case TINH_CHAT_KY_HAN.KHONG_KY_HAN_TU_NGUYEN: return "KKHTN";
                case TINH_CHAT_KY_HAN.KHONG_KY_HAN_QUY_DINH_1: return "KKHQ1";
                case TINH_CHAT_KY_HAN.KHONG_KY_HAN_TRON_THANG: return "KKH1T";
                default: return "";
            }
        }

        //Loại hình lập kế hoạch
        public enum LOAI_HINH_LAP_KE_HOACH
        {
            GOC,
            LAI,
            PHI
        }

        public static string layGiaTri(this LOAI_HINH_LAP_KE_HOACH loaiHinhLapKeHoach)
        {
            switch (loaiHinhLapKeHoach)
            {
                case LOAI_HINH_LAP_KE_HOACH.GOC: return "GOC";
                case LOAI_HINH_LAP_KE_HOACH.LAI: return "LAI";
                case LOAI_HINH_LAP_KE_HOACH.PHI: return "PHI";
                default: return "";
            }
        }

        // Đơn vị tính lịch họp
        public enum LHOP_DVI_TINH_TOAN
        {
            NGAY,
            THU
        }

        public static string layGiaTri(this LHOP_DVI_TINH_TOAN dViLichHop)
        {
            switch (dViLichHop)
            {
                case LHOP_DVI_TINH_TOAN.NGAY: return "NGAY";
                case LHOP_DVI_TINH_TOAN.THU: return "THU";
                default: return "";
            }
        }

        // Trạng thái kế hoạch
        public enum TTHAI_KHOACH
        {
            DTT,
            CTT,
            T1P,
            TTR,
            KPT
        }

        public static string layGiaTri(this TTHAI_KHOACH trangThai)
        {
            switch (trangThai)
            {
                case TTHAI_KHOACH.DTT: return "DTT";
                case TTHAI_KHOACH.CTT: return "CTT";
                case TTHAI_KHOACH.T1P: return "T1P";
                case TTHAI_KHOACH.TTR: return "TTR";
                case TTHAI_KHOACH.KPT: return "KPT";
                default: return "";
            }
        }

        // Trạng thái kế hoạch
        public enum NGAY_GD_ROI_VAO_NGAY_NGHI
        {
            NGAY_TRUOC,
            NGAY_SAU,
            GIU_NGUYEN
        }

        public static string layGiaTri(this NGAY_GD_ROI_VAO_NGAY_NGHI trangThai)
        {
            switch (trangThai)
            {
                case NGAY_GD_ROI_VAO_NGAY_NGHI.NGAY_TRUOC: return "BD";
                case NGAY_GD_ROI_VAO_NGAY_NGHI.NGAY_SAU: return "ND";
                case NGAY_GD_ROI_VAO_NGAY_NGHI.GIU_NGUYEN: return "NDF";
                default: return "";
            }
        }

        //Cơ sở tính lãi
        public enum CO_SO_TINH_LAI
        {
            CS360_360,
            CS366_360,
            CS366_366,
            CS360_366
        }

        public static string layGiaTri(this CO_SO_TINH_LAI coSo)
        {
            switch (coSo)
            {
                case CO_SO_TINH_LAI.CS360_360: return "360/360";
                case CO_SO_TINH_LAI.CS366_360: return "366/360";
                case CO_SO_TINH_LAI.CS360_366: return "360/366";
                case CO_SO_TINH_LAI.CS366_366: return "366/366";
                default: return "";
            }
        }

        //Nguyên nhân thay đổi 
        public enum NGUYEN_NHAN_THAY_DOI_LTN
        {
            DIEU_CHINH_LAI_SUAT,
            GIA_HAN_NO,
            CO_CAU_KY_HAN,
            THU_NO_TRUOC_HAN
        }

        public static string layGiaTri(this NGUYEN_NHAN_THAY_DOI_LTN nguyenNhan)
        {
            switch (nguyenNhan)
            {
                case NGUYEN_NHAN_THAY_DOI_LTN.DIEU_CHINH_LAI_SUAT: return "DIEU_CHINH_LAI_SUAT";
                case NGUYEN_NHAN_THAY_DOI_LTN.GIA_HAN_NO: return "GIA_HAN_NO";
                case NGUYEN_NHAN_THAY_DOI_LTN.CO_CAU_KY_HAN: return "CO_CAU_KY_HAN";
                case NGUYEN_NHAN_THAY_DOI_LTN.THU_NO_TRUOC_HAN: return "THU_NO_TRUOC_HAN";
                default: return "";
            }
        }

        //Nguyên nhân thay đổi 
        public enum TINH_CHAT_VONG_VAY
        {
            CO_DINH,
            THAY_DOI
        }

        public static string layGiaTri(this TINH_CHAT_VONG_VAY TINH_CHAT_VONG_VAY)
        {
            switch (TINH_CHAT_VONG_VAY)
            {
                case TINH_CHAT_VONG_VAY.CO_DINH: return "CO_DINH";
                case TINH_CHAT_VONG_VAY.THAY_DOI: return "THAY_DOI";
                default: return "";
            }
        }

        //Phương thức in 
        public enum PHUONG_THUC_IN
        {
            CHI_TIET,
            PHAN_LOAI_TKHOAN,
            LOAI_CHUNG_TU,
            GOP
        }

        public static string layGiaTri(this PHUONG_THUC_IN PHUONG_THUC_IN)
        {
            switch (PHUONG_THUC_IN)
            {
                case PHUONG_THUC_IN.CHI_TIET: return "CHI_TIET";
                case PHUONG_THUC_IN.PHAN_LOAI_TKHOAN: return "PHAN_LOAI_TKHOAN";
                case PHUONG_THUC_IN.LOAI_CHUNG_TU: return "LOAI_CHUNG_TU";
                case PHUONG_THUC_IN.GOP: return "GOP";
                default: return "";
            }
        }

        //Loại giấy tờ 
        public enum LOAI_GIAY_TO
        {
            CHUNG_MINH_ND,
            HO_CHIEU,
            BANG_LAI_XE,
            GP_DKKD,
            GIAY_TO_KHAC
        }

        public static string layGiaTri(this LOAI_GIAY_TO loai)
        {
            switch (loai)
            {
                case LOAI_GIAY_TO.CHUNG_MINH_ND: return "CHUNG_MINH_ND";
                case LOAI_GIAY_TO.HO_CHIEU: return "HO_CHIEU";
                case LOAI_GIAY_TO.BANG_LAI_XE: return "BANG_LAI_XE";
                case LOAI_GIAY_TO.GP_DKKD: return "GP_DKKD";
                case LOAI_GIAY_TO.GIAY_TO_KHAC: return "GIAY_TO_KHAC";
                default: return "";
            }
        }

        //Loại địa chỉ (MAC & IP)
        public enum LOAI_DIA_CHI
        {
            MAC,
            IP
        }

        public static string layGiaTri(this LOAI_DIA_CHI loai)
        {
            switch (loai)
            {
                case LOAI_DIA_CHI.MAC: return "MAC";
                case LOAI_DIA_CHI.IP: return "IP";
                default: return "";
            }
        }

        //Loại phạm vi
        public enum LOAI_PHAM_VI
        {
            DON_VI,
            DIA_LY
        }

        public static string layGiaTri(this LOAI_PHAM_VI loai)
        {
            switch (loai)
            {
                case LOAI_PHAM_VI.DON_VI: return "DON_VI";
                case LOAI_PHAM_VI.DIA_LY: return "DIA_LY";
                default: return "";
            }
        }

        //Trạng thái mặc định hay không mặc định
        public enum TRANG_THAI_MAC_DINH
        {
            MAC_DINH,
            KHONG_MAC_DINH
        }

        public static string layGiaTri(this TRANG_THAI_MAC_DINH trangThai)
        {
            switch (trangThai)
            {
                case TRANG_THAI_MAC_DINH.MAC_DINH: return "MDI";
                case TRANG_THAI_MAC_DINH.KHONG_MAC_DINH: return "KMD";
                default: return "";
            }
        }

        //Loại hồ sơ nhân sự
        public enum LOAI_HO_SO
        {
            TUYEN_DUNG,
            CONG_TAC_VIEN,
            HOC_VIEC,
            THU_VIEC,
            CHINH_THUC,
            THOI_VIEC
        }

        public static string layGiaTri(this LOAI_HO_SO loaiHoSo)
        {
            switch (loaiHoSo)
            {
                case LOAI_HO_SO.TUYEN_DUNG: return "TUYEN_DUNG";
                case LOAI_HO_SO.CONG_TAC_VIEN: return "CONG_TAC_VIEN";
                case LOAI_HO_SO.HOC_VIEC: return "HOC_VIEC";
                case LOAI_HO_SO.THU_VIEC: return "THU_VIEC";
                case LOAI_HO_SO.CHINH_THUC: return "CHINH_THUC";
                case LOAI_HO_SO.THOI_VIEC: return "THOI_VIEC";
                default: return "";
            }
        }

        //Loại hợp đồng lao động
        public enum LOAI_HDLD
        {            
            CONG_TAC_VIEN,
            HOC_VIEC,
            THU_VIEC,
            CHINH_THUC
        }

        public static string layGiaTri(this LOAI_HDLD loaiHDLD)
        {
            switch (loaiHDLD)
            {                
                case LOAI_HDLD.CONG_TAC_VIEN: return "CONG_TAC_VIEN";
                case LOAI_HDLD.HOC_VIEC: return "HOC_VIEC";
                case LOAI_HDLD.THU_VIEC: return "THU_VIEC";
                case LOAI_HDLD.CHINH_THUC: return "CHINH_THUC";
                default: return "";
            }
        }

        //Loại thời hạn hợp đồng lao đồng
        public enum LOAI_THOI_HAN_HDLD
        {
            CO_THOI_HAN,
            KHONG_THOI_HAN
        }

        public static string layGiaTri(this LOAI_THOI_HAN_HDLD loaiThoiHanHDLD)
        {
            switch (loaiThoiHanHDLD)
            {
                case LOAI_THOI_HAN_HDLD.CO_THOI_HAN: return "CO_THOI_HAN";
                case LOAI_THOI_HAN_HDLD.KHONG_THOI_HAN: return "KHONG_THOI_HAN";
                default: return "";
            }
        }

        //Loại tác động lên hồ sơ
        public enum LOAI_TAC_DONG_HSO
        {
            HOP_DONG,
            CHUYEN_CTAC,
            THOI_VIEC
        }

        public static string layGiaTri(this LOAI_TAC_DONG_HSO loai)
        {
            switch (loai)
            {
                case LOAI_TAC_DONG_HSO.HOP_DONG: return "HOP_DONG";
                case LOAI_TAC_DONG_HSO.CHUYEN_CTAC: return "CHUYEN_CTAC";
                case LOAI_TAC_DONG_HSO.THOI_VIEC: return "THOI_VIEC";
                default: return "";
            }
        }

        //Loại tác động lên hồ sơ
        public enum LOAI_HAN_MUC
        {
            CHUNG,
            CHI_TIET
        }

        public static string layGiaTri(this LOAI_HAN_MUC loai)
        {
            switch (loai)
            {
                case LOAI_HAN_MUC.CHUNG: return "CHUNG";
                case LOAI_HAN_MUC.CHI_TIET: return "CHI_TIET";                
                default: return "";
            }
        }

        public enum LOAI_HO_NGHEO
        {
            NGHEO,
            CAN_NGHEO,
            KHONG
        }

        public static string layGiaTri(this LOAI_HO_NGHEO loai)
        {
            switch (loai)
            {
                case LOAI_HO_NGHEO.NGHEO: return "NGHEO";
                case LOAI_HO_NGHEO.CAN_NGHEO: return "CAN_NGHEO";
                case LOAI_HO_NGHEO.KHONG: return "KHONG";
                default: return "KHONG";
            }
        }

        public enum NHA_O_LOAI
        {
            MAI_NHA,
            TUONG,
            NEN,
            SAN,
            NUOC,
            ANH_SANG            
        }

        public static string layGiaTri(this NHA_O_LOAI loai)
        {
            switch (loai)
            {
                case NHA_O_LOAI.MAI_NHA: return "MAI_NHA";
                case NHA_O_LOAI.TUONG: return "TUONG";
                case NHA_O_LOAI.NEN: return "NEN";
                case NHA_O_LOAI.SAN: return "SAN";
                case NHA_O_LOAI.NUOC: return "NUOC";
                case NHA_O_LOAI.ANH_SANG: return "ANH_SANG";
                default: return "";
            }
        }

        public enum NHA_O_MAI_NHA
        {
            NGOI,
            TON,
            XI_MANG,
            TRANH_LA,
            KHAC
        }

        public static string layGiaTri(this NHA_O_MAI_NHA loai)
        {
            switch (loai)
            {
                case NHA_O_MAI_NHA.NGOI: return "NGOI";
                case NHA_O_MAI_NHA.TON: return "TON";
                case NHA_O_MAI_NHA.XI_MANG: return "XI_MANG";
                case NHA_O_MAI_NHA.TRANH_LA: return "TRANH_LA";
                case NHA_O_MAI_NHA.KHAC: return "KHAC";
                default: return "KHAC";
            }
        }

        public enum NHA_O_TUONG
        {
            XAY,
            TON,
            VAN,
            LA,
            KHAC
        }

        public static string layGiaTri(this NHA_O_TUONG loai)
        {
            switch (loai)
            {
                case NHA_O_TUONG.XAY: return "XAY";
                case NHA_O_TUONG.TON: return "TON";
                case NHA_O_TUONG.VAN: return "VAN";
                case NHA_O_TUONG.LA: return "LA";
                case NHA_O_TUONG.KHAC: return "KHAC";
                default: return "KHAC";
            }
        }

        public enum NHA_O_NEN
        {
            GACH_MEN,
            GACH_TAU,
            XI_MANG,
            DAT,
            KHAC
        }

        public static string layGiaTri(this NHA_O_NEN loai)
        {
            switch (loai)
            {
                case NHA_O_NEN.GACH_MEN: return "GACH_MEN";
                case NHA_O_NEN.GACH_TAU: return "GACH_TAU";
                case NHA_O_NEN.XI_MANG: return "XI_MANG";
                case NHA_O_NEN.DAT: return "DAT";
                case NHA_O_NEN.KHAC: return "KHAC";
                default: return "KHAC";
            }
        }

        public enum NHA_O_SAN
        {
            GACH,
            XI_MANG,
            DAT,
            CAT,
            KHAC
        }

        public static string layGiaTri(this NHA_O_SAN loai)
        {
            switch (loai)
            {
                case NHA_O_SAN.GACH: return "GACH";
                case NHA_O_SAN.XI_MANG: return "XI_MANG";
                case NHA_O_SAN.DAT: return "DAT";
                case NHA_O_SAN.CAT: return "CAT";
                case NHA_O_SAN.KHAC: return "KHAC";
                default: return "KHAC";
            }
        }

        public enum NHA_O_NUOC
        {
            NUOC_MAY,
            CAY_NUOC,
            BE_LU_VAI
        }

        public static string layGiaTri(this NHA_O_NUOC loai)
        {
            switch (loai)
            {
                case NHA_O_NUOC.NUOC_MAY: return "NUOC_MAY";
                case NHA_O_NUOC.CAY_NUOC: return "CAY_NUOC";
                case NHA_O_NUOC.BE_LU_VAI: return "BE_LU_VAI";
                default: return "NUOC_MAY";
            }
        }

        public enum NHA_O_ANH_SANG
        {
            DIEN,
            DAU
        }

        public static string layGiaTri(this NHA_O_ANH_SANG loai)
        {
            switch (loai)
            {
                case NHA_O_ANH_SANG.DIEN: return "DIEN";
                case NHA_O_ANH_SANG.DAU: return "DAU";
                default: return "DIEN";
            }
        }

        public enum TINH_CHAT_SDUNG_DAT
        {
            SO_HUU,
            THUE,
            KHAC
        }

        public static string layGiaTri(this TINH_CHAT_SDUNG_DAT loai)
        {
            switch (loai)
            {
                case TINH_CHAT_SDUNG_DAT.SO_HUU: return "SO_HUU";
                case TINH_CHAT_SDUNG_DAT.THUE: return "THUE";
                case TINH_CHAT_SDUNG_DAT.KHAC: return "KHAC";
                default: return "KHAC";
            }
        }

        public enum LOAI_BIEN_DONG_TSAN
        {
            TANG,
            GIAM,
            BAN_GIAO,
            NANG_CAP,
            DANH_GIA,
            KHAU_HAO
        }

        public static string layGiaTri(this LOAI_BIEN_DONG_TSAN loai)
        {
            switch (loai)
            {
                case LOAI_BIEN_DONG_TSAN.TANG: return "TANG";
                case LOAI_BIEN_DONG_TSAN.GIAM: return "GIAM";
                case LOAI_BIEN_DONG_TSAN.BAN_GIAO: return "BAN_GIAO";
                case LOAI_BIEN_DONG_TSAN.NANG_CAP: return "NANG_CAP";
                case LOAI_BIEN_DONG_TSAN.DANH_GIA: return "DANH_GIA";
                case LOAI_BIEN_DONG_TSAN.KHAU_HAO: return "KHAU_HAO";
                default: return "TANG";
            }
        }

        public enum LOAI_PHU_CAP
        {
            CO_DINH,
            BO_SUNG            
        }

        public static string layGiaTri(this LOAI_PHU_CAP loai)
        {
            switch (loai)
            {
                case LOAI_PHU_CAP.CO_DINH: return "CO_DINH";
                case LOAI_PHU_CAP.BO_SUNG: return "BO_SUNG";
                default: return "";
            }
        }

        public enum LOAI_DANH_GIA_TAI_SAN
        {
            TANG,
            GIAM
        }

        public static string layGiaTri(this LOAI_DANH_GIA_TAI_SAN loai)
        {
            switch (loai)
            {
                case LOAI_DANH_GIA_TAI_SAN.TANG: return "01";
                case LOAI_DANH_GIA_TAI_SAN.GIAM: return "02";
                default: return "";
            }
        }

        public enum NGUYEN_NHAN_DANH_GIA_TAI_SAN
        {
            THAO_DO_1_PHAN
        }

        public static string layGiaTri(this NGUYEN_NHAN_DANH_GIA_TAI_SAN loai)
        {
            switch (loai)
            {
                case NGUYEN_NHAN_DANH_GIA_TAI_SAN.THAO_DO_1_PHAN: return "02";
                default: return "";
            }
        }

        public enum NGUYEN_NHAN_GIAM_TAI_SAN
        {
            THANH_LY,
            BAO_MAT
        }

        public static string layGiaTri(this NGUYEN_NHAN_GIAM_TAI_SAN loai)
        {
            switch (loai)
            {
                case NGUYEN_NHAN_GIAM_TAI_SAN.THANH_LY: return "01";
                case NGUYEN_NHAN_GIAM_TAI_SAN.BAO_MAT: return "02";
                default: return "";
            }
        }

        public enum LOAI_TAI_SAN
        {
            HUU_HINH,
            VO_HINH
        }

        public static string layGiaTri(this LOAI_TAI_SAN loai)
        {
            switch (loai)
            {
                case LOAI_TAI_SAN.HUU_HINH: return "01";
                case LOAI_TAI_SAN.VO_HINH: return "02";
                default: return "";
            }
        }


        public enum HINH_THUC_NHAP_TS
        {
            DO_DANG,
            HOAN_THANH
        }

        public static string layGiaTri(this HINH_THUC_NHAP_TS loai)
        {
            switch (loai)
            {
                case HINH_THUC_NHAP_TS.DO_DANG: return "01";
                case HINH_THUC_NHAP_TS.HOAN_THANH: return "02";
                default: return "";
            }
        }


        public enum HINH_THUC_BAN_GIAO_TS
        {
            CHINH_THUC,
            TAM_THOI
        }

        public static string layGiaTri(this HINH_THUC_BAN_GIAO_TS loai)
        {
            switch (loai)
            {
                case HINH_THUC_BAN_GIAO_TS.CHINH_THUC: return "01";
                case HINH_THUC_BAN_GIAO_TS.TAM_THOI: return "02";
                default: return "";
            }
        }

        public enum HTHUC_PBO_CPHI_TS
        {
            CHI_PHI_TRUC_TIEP,
            CHI_PHI_CHO_PBO
        }

        public static string layGiaTri(this HTHUC_PBO_CPHI_TS loai)
        {
            switch (loai)
            {
                case HTHUC_PBO_CPHI_TS.CHI_PHI_TRUC_TIEP: return "01";
                case HTHUC_PBO_CPHI_TS.CHI_PHI_CHO_PBO: return "02";
                default: return "";
            }
        }

        public enum PP_KHAU_HAO
        {
            DUONG_THANG,
            SDU_GIAM_DAN,
            SLUONG_KLUONG
        }

        public static string layGiaTri(this PP_KHAU_HAO pphap)
        {
            switch (pphap)
            {
                case PP_KHAU_HAO.DUONG_THANG: return "01";
                case PP_KHAU_HAO.SDU_GIAM_DAN: return "02";
                case PP_KHAU_HAO.SLUONG_KLUONG: return "03";
                default: return "";
            }
        }

        /// <summary>
        /// Danh mục Loại đối tượng và đối tượng
        /// </summary>
        public enum DM_Loai_DTuong { LOAI_DOI_TUONG, DOI_TUONG };
        /// <summary>
        /// Lấy giá trị kiểu string của enum
        /// </summary>
        /// <param name="loaiDoiTuong">Enum cần lấy giá trị kiểu string</param>
        /// <returns>Trả lại chuỗi chứa giá trị kiểu string</returns>
        public static string layGiaTri(this DM_Loai_DTuong loaiDoiTuong)
        {
            switch (loaiDoiTuong)
            {
                case DM_Loai_DTuong.LOAI_DOI_TUONG: return "LOAI_DOI_TUONG";
                case DM_Loai_DTuong.DOI_TUONG: return "DOI_TUONG";
                default: return "";
            }
        }


        //Nhóm chức vụ cộng tác viên
        public enum NHOM_CTV
        {
            BQLTP,
            BQLCH,
            BQLCX,
            BQLCA
        }

        public static string layGiaTri(this NHOM_CTV nhomCTV)
        {
            switch (nhomCTV)
            {
                case NHOM_CTV.BQLTP: return "BQLTP";
                case NHOM_CTV.BQLCH: return "BQLCH";
                case NHOM_CTV.BQLCX: return "BQLCX";
                case NHOM_CTV.BQLCA: return "BQLCA";
                default: return "";
            }
        }

        //Nhóm chức vụ cộng tác viên
        public enum TTHAI_TSAN
        {
            CHUA_HTOAN, //Chua hach toan ngoai bang
            DA_HTOAN, //Da hach toan ngoai bang
            DA_XUAT //Da xuat tai san
        }

        public static string layGiaTri(this TTHAI_TSAN tthaiTSan)
        {
            switch (tthaiTSan)
            {
                case TTHAI_TSAN.CHUA_HTOAN: return "CHUA_HTOAN";
                case TTHAI_TSAN.DA_HTOAN: return "DA_HTOAN";
                case TTHAI_TSAN.DA_XUAT: return "DA_XUAT";
                default: return "";
            }
        }

        public enum CACH_TINH_SO_NGAY_TD
        {
            TAT_CA,
            THEM_NGAY_DAU,
            THEM_NGAY_CUOI,
            NGAY_THUC_TE
        }

        public static string layGiaTri(this CACH_TINH_SO_NGAY_TD loai)
        {
            switch (loai)
            {
                case CACH_TINH_SO_NGAY_TD.TAT_CA: return "TAT_CA";
                case CACH_TINH_SO_NGAY_TD.THEM_NGAY_DAU: return "THEM_NGAY_DAU";
                case CACH_TINH_SO_NGAY_TD.THEM_NGAY_CUOI: return "THEM_NGAY_CUOI";
                case CACH_TINH_SO_NGAY_TD.NGAY_THUC_TE: return "NGAY_THUC_TE";
                default: return "";
            }
        }

        public enum HINH_THUC_DANG_KY
        {
            MOT_PHAN,
            TAT_TOAN,
        }

        public static string layGiaTri(this HINH_THUC_DANG_KY loai)
        {
            switch (loai)
            {
                case HINH_THUC_DANG_KY.MOT_PHAN: return "MOT_PHAN";
                case HINH_THUC_DANG_KY.TAT_TOAN: return "TAT_TOAN";
                default: return "";
            }
        }

        public enum PartDebit
        {
            PART_0,
            PART_1,
            PART_2,
            PART_3,
            PART_4,
            PART_5
        }

        public static string layGiaTri(this PartDebit part)
        {
            switch (part)
            {
                case PartDebit.PART_0: return "part0";
                case PartDebit.PART_1: return "part1";
                case PartDebit.PART_2: return "part2";
                case PartDebit.PART_3: return "part3";
                case PartDebit.PART_4: return "part4";
                case PartDebit.PART_5: return "part5";
                default: return "";
            }
        }

        public static string layGiaTriNgonNgu(this PartDebit part)
        {
            switch (part)
            {
                case PartDebit.PART_0: return "U.DungChung.Part0";
                case PartDebit.PART_1: return "U.DungChung.Part1";
                case PartDebit.PART_2: return "U.DungChung.Part2";
                case PartDebit.PART_3: return "U.DungChung.Part3";
                case PartDebit.PART_4: return "U.DungChung.Part4";
                case PartDebit.PART_5: return "U.DungChung.Part5";
                default: return "U.DungChung.Part0";
            }
        }

        public enum LOAI_TIEN_TE
        {
            EUR,
            USD,
            CNY,
            MMK,
            SGD,
            VND,
            LAK,
            DEFAULT
        }

        public static string getValue(this LOAI_TIEN_TE loaiTien)
        {
            switch (loaiTien)
            {
                case LOAI_TIEN_TE.EUR: return "EUR";
                case LOAI_TIEN_TE.USD: return "USD";
                case LOAI_TIEN_TE.CNY: return "CNY";
                case LOAI_TIEN_TE.MMK: return "MMK";
                case LOAI_TIEN_TE.SGD: return "SGD";
                case LOAI_TIEN_TE.VND: return "VND";
                case LOAI_TIEN_TE.LAK: return "LAK";
                default: return "";
            }
        }

        public static LOAI_TIEN_TE layLoaiTienTe(string loaiTienTe)
        {
            switch (loaiTienTe)
            {
                case "EUR": return LOAI_TIEN_TE.EUR;
                case "USD": return LOAI_TIEN_TE.USD;
                case "CNY": return LOAI_TIEN_TE.CNY;
                case "MMK": return LOAI_TIEN_TE.MMK;
                case "SGD": return LOAI_TIEN_TE.SGD;
                case "VND": return LOAI_TIEN_TE.VND;
                case "LAK": return LOAI_TIEN_TE.LAK;
                default: return LOAI_TIEN_TE.DEFAULT;
            }
        }

        public static string getSinhMa(this LOAI_TIEN_TE loaiTien)
        {
            switch (loaiTien)
            {
                case LOAI_TIEN_TE.EUR: return "";
                case LOAI_TIEN_TE.USD: return "1";
                case LOAI_TIEN_TE.CNY: return "";
                case LOAI_TIEN_TE.MMK: return "0";
                case LOAI_TIEN_TE.SGD: return "2";
                case LOAI_TIEN_TE.VND: return "";
                case LOAI_TIEN_TE.LAK: return "0";
                default: return "";
            }
        }

        public enum PHUONG_THUC_CHIA_DEU_GOC
        {
            NGAY,
            KY_TRA_NO
        }

        public static string getValue(this PHUONG_THUC_CHIA_DEU_GOC phuongThucChiaDeuGoc)
        {
            switch (phuongThucChiaDeuGoc)
            {
                case PHUONG_THUC_CHIA_DEU_GOC.NGAY: return "NGAY";
                case PHUONG_THUC_CHIA_DEU_GOC.KY_TRA_NO: return "KY_TRA_NO";
                default: return "";
            }
        }

        public enum LAM_TRON_LAI_KE_HOACH
        {
            TONG_LAI,
            HANG_KY,
            CA_HAI,
            GIU_NGUYEN
        }

        public static string getValue(this LAM_TRON_LAI_KE_HOACH lamTronLaiKeHoach)
        {
            switch (lamTronLaiKeHoach)
            {
                case LAM_TRON_LAI_KE_HOACH.TONG_LAI: return "TONG_LAI";
                case LAM_TRON_LAI_KE_HOACH.HANG_KY: return "HANG_KY";
                case LAM_TRON_LAI_KE_HOACH.CA_HAI: return "CA_HAI";
                case LAM_TRON_LAI_KE_HOACH.GIU_NGUYEN: return "GIU_NGUYEN";
                default: return "";
            }
        }

        public enum LAM_TRON_GOC_KE_HOACH
        {
            TONG_LAI,
            HANG_KY,
            CA_HAI,
            GIU_NGUYEN
        }

        public static string getValue(this LAM_TRON_GOC_KE_HOACH lamTronGocKeHoach)
        {
            switch (lamTronGocKeHoach)
            {
                case LAM_TRON_GOC_KE_HOACH.TONG_LAI: return "TONG_LAI";
                case LAM_TRON_GOC_KE_HOACH.HANG_KY: return "HANG_KY";
                case LAM_TRON_GOC_KE_HOACH.CA_HAI: return "CA_HAI";
                case LAM_TRON_GOC_KE_HOACH.GIU_NGUYEN: return "GIU_NGUYEN";
                default: return "";
            }
        }

        public enum PHUONG_THUC_LAM_TRON
        {
            LEN,
            XUONG,
            CA_HAI,
            GIU_NGUYEN
        }

        public static string getValue(this PHUONG_THUC_LAM_TRON phuongThucLamTron)
        {
            switch (phuongThucLamTron)
            {
                case PHUONG_THUC_LAM_TRON.LEN: return "LEN";
                case PHUONG_THUC_LAM_TRON.XUONG: return "XUONG";
                case PHUONG_THUC_LAM_TRON.CA_HAI: return "CA_HAI";
                case PHUONG_THUC_LAM_TRON.GIU_NGUYEN: return "GIU_NGUYEN";
                default: return "";
            }
        }

        public static PHUONG_THUC_LAM_TRON getPhuongThucLamTron(string phuongThucLamTron)
        {
            switch (phuongThucLamTron)
            {
                case "LEN": return PHUONG_THUC_LAM_TRON.LEN;
                case "XUONG": return PHUONG_THUC_LAM_TRON.XUONG;
                case "CA_HAI": return PHUONG_THUC_LAM_TRON.CA_HAI;
                case "GIU_NGUYEN": return PHUONG_THUC_LAM_TRON.GIU_NGUYEN;
                default: return PHUONG_THUC_LAM_TRON.GIU_NGUYEN;
            }
        }

        public enum TRANG_THAI_XU_LY
        {
            CHO_XU_LY,
            DA_XU_LY,
            KHONG_XU_LY,
            DEFAULT
        }

        public static string getValue(this TRANG_THAI_XU_LY trangThaiXuLy)
        {
            switch (trangThaiXuLy)
            {
                case TRANG_THAI_XU_LY.CHO_XU_LY: return "CXL";
                case TRANG_THAI_XU_LY.DA_XU_LY: return "DXL";
                case TRANG_THAI_XU_LY.KHONG_XU_LY: return "KXL";
                default: return "";
            }
        }

        public static TRANG_THAI_XU_LY getTrangThaiXuLy(string trangThaiXuLy)
        {
            switch(trangThaiXuLy)
            {
                case "CXL": return TRANG_THAI_XU_LY.CHO_XU_LY;
                case "DXL": return TRANG_THAI_XU_LY.DA_XU_LY;
                case "KXL": return TRANG_THAI_XU_LY.KHONG_XU_LY;
                default: return TRANG_THAI_XU_LY.DEFAULT;
            }
        }
    }
}
