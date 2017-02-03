using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Common;
namespace Presentation.WebClient.Business.CustomControl
{
    public class CommonFuntion
    {
        /// <summary>
        /// Lấy trạng thái bản ghi dựa theo action và trạng thái bản ghi hiện tại
        /// </summary>
        /// <returns></returns>
        public static string LayTrangThaiBanGhi(DatabaseConstant.Action action, BusinessConstant.TrangThaiNghiepVu status)
        {
            string trangthai = "";
            switch (action)
            {
                case DatabaseConstant.Action.LUU_TAM:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.CHO_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET);
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.LUU:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.TRINH_DUYET:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    break;
                case DatabaseConstant.Action.THOAI_DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    break;
                case DatabaseConstant.Action.TU_CHOI_DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    break;
            }
            return trangthai;
        }
    }
}