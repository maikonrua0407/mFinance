using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using BusinessServices.Utilities.DTO;
using BusinessServices.QuanTriHeThong.DTO;
using BusinessServices.QuanTriHeThong.Detail;
using DataModel.EntityFramework;

namespace BusinessServices.QuanTriHeThong.Action
{
    public class BS_QuanTriHeThong_Action
    {
        /// <summary>
        /// Lưu phân quyền phạm vi
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public ApplicationConstant.ResponseStatus PhamVi_Luu(ref THONG_TIN_CHUNG objThongTinChung, ref PHAM_VI obj, ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage, ref BS_ResponseDetail bsRetDetail)
        {

            ApplicationConstant.NghiepVuResponseMessage responseMessageNV = ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_ThanhCong;

            //Kiểm tra hợp lệ hệ thống
            if (new BS_DungChung_Detail().KiemTraHopLeHeThong(ref responseMessageNV) != 0)
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            //Kiểm tra chuẩn bị dữ liệu
            if (new BS_PhamVi_Detail().KiemTraVaChuanBiDuLieu(ref objThongTinChung, ref obj, ref responseMessage) != 1)
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

            //Xử lý nghiệp vụ
            return new BS_PhamVi_Detail().Luu(ref obj, ref responseMessage, ref bsRetDetail);
        }

        /// <summary>
        /// Xóa phân quyền phạm vi
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public ApplicationConstant.ResponseStatus PhamVi_Xoa(ref THONG_TIN_CHUNG objThongTinChung, ref PHAM_VI obj, ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage, ref BS_ResponseDetail bsRetDetail)
        {
            ApplicationConstant.NghiepVuResponseMessage responseMessageNV = ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_ThanhCong;

            //Kiểm tra hợp lệ hệ thống
            if (new BS_DungChung_Detail().KiemTraHopLeHeThong(ref responseMessageNV) != 0)
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            //Kiểm tra chuẩn bị dữ liệu
            if (new BS_PhamVi_Detail().KiemTraVaChuanBiDuLieu(ref objThongTinChung, ref obj, ref responseMessage) != 1)
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

            //Xử lý nghiệp vụ
            return new BS_PhamVi_Detail().Xoa(ref obj, ref responseMessage, ref bsRetDetail);
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public ApplicationConstant.ResponseStatus PhamVi_LayDuLieu(ref THONG_TIN_CHUNG objThongTinChung, ref PHAM_VI obj, ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage, ref BS_ResponseDetail bsRetDetail)
        {
            ApplicationConstant.NghiepVuResponseMessage responseMessageNV = ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_ThanhCong;

            //Kiểm tra hợp lệ hệ thống
            if (new BS_DungChung_Detail().KiemTraHopLeHeThong(ref responseMessageNV) != 0)
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            //Xử lý nghiệp vụ
            return new BS_PhamVi_Detail().LayDuLieu(ref obj, ref responseMessage, ref bsRetDetail);
        }

        public ApplicationConstant.ResponseStatus MaTranPheDuyet_Luu(ref THONG_TIN_CHUNG objThongTinChung, ref HT_CNANG obj, ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage, ref BS_ResponseDetail bsRetDetail)
        {            
            //Xử lý nghiệp vụ
            return new BS_ChucNang_Detail().LuuMaTranPheDuyet(ref objThongTinChung, ref obj, ref responseMessage, ref bsRetDetail);
        }
    }
}
