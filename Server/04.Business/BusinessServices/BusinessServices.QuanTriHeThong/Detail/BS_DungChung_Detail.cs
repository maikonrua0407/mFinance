using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using BusinessServices.Utilities;

namespace BusinessServices.QuanTriHeThong.Detail
{
    public class BS_DungChung_Detail
    {
        /// <summary>
        /// Trả về 0 nếu hệ thống đang được phép giao dịch
        /// </summary>
        /// <param name="responseMessage"> Message tương ứng</param>
        /// <returns></returns>
        public int KiemTraHopLeHeThong(ref ApplicationConstant.NghiepVuResponseMessage responseMessage)
        {
            int iret = 0; //Kết quả trả về
            BS_CoHeThong bsCoHeThong = new BS_CoHeThong();

            //Kiểm tra trạng thái hệ thống, có được giao dịch không?
            switch (bsCoHeThong.KiemTraTrangThaiHeThong())
            {
                case -1:
                    iret = -1;
                    responseMessage = ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_HeThong_TamNgungGiaoDich;
                    break;
                case -2:
                    iret = -1;
                    responseMessage = ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_HeThong_NgungGiaoDich;
                    break;
                default:
                    iret = 0;
                    responseMessage = ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_ThanhCong;
                    break;
            }

            return iret;
        }
    }
}
