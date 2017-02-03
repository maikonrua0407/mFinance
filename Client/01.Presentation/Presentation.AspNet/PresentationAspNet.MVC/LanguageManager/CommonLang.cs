using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Common;

namespace PresentationAspNet.MVC.LanguageManager
{
    public class CommonLang
    {
        /// <summary>
        /// Lấy ngôn ngữ hiển thị trạng thái nghiệp vụ
        /// </summary>
        /// <param name="trangthai">string TrangThaiNghiepVu</param>
        /// <returns>trạng thái nghiệp vụ</returns>
        public static string layNgonNguNghiepVu(string trangthai)
        {
            if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.LuuTam");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.ChoDuyet");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.SuaSauDuyet");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.DA_DUYET))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.DaDuyet");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.TU_CHOI))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.TuChoi");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.ThoaiDuyet");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.LuuTamSuaSauDuyet");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.TuChoiSuaSauDuyet");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.TU_CHOI_CAP_TIN_DUNG))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiNghiepVu.TuChoi");
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
            if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiBanGhi.SU_DUNG))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiBanGhi.SuDung");
            }
            else if (trangthai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiBanGhi.KHONG_SU_DUNG))
            {
                return LanguageNode.GetValueUILanguage("U.DungChung.TrangThaiBanGhi.KhongSuDung");
            }

            return "";
        }
    }
}