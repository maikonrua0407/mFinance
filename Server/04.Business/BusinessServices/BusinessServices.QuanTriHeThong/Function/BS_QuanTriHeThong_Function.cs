using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using BusinessServices.Utilities.DTO;
using BusinessServices.QuanTriHeThong.DTO;
using BusinessServices.QuanTriHeThong.Action;
using DataModel.EntityFramework;

namespace BusinessServices.QuanTriHeThong.Function
{
    public class BS_QuanTriHeThong_Function
    {
        public ApplicationConstant.ResponseStatus PhanQuyenPhamVi(DatabaseConstant.Action action,
                                                       ref THONG_TIN_CHUNG objThongTinChung,
                                                       ref List<BS_ResponseDetail> lstResponseDetail,
                                                       ref PHAM_VI objPhamVi,
                                                       ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            ApplicationConstant.ResponseStatus responseStatus = new ApplicationConstant.ResponseStatus();
            BS_ResponseDetail responseDetail = new BS_ResponseDetail();
            switch (action)
            {
                case DatabaseConstant.Action.XOA: //Xóa
                    responseStatus = new BS_QuanTriHeThong_Action().PhamVi_Xoa(ref objThongTinChung, ref objPhamVi, ref responseMessage, ref responseDetail);
                    break;
                case DatabaseConstant.Action.LUU://Thêm
                    responseStatus = new BS_QuanTriHeThong_Action().PhamVi_Luu(ref objThongTinChung, ref objPhamVi, ref responseMessage, ref responseDetail);
                    break;
                case DatabaseConstant.Action.LAY_LAI://Sua
                    responseStatus = new BS_QuanTriHeThong_Action().PhamVi_LayDuLieu(ref objThongTinChung, ref objPhamVi, ref responseMessage, ref responseDetail);
                    break;
                default:
                    responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                    break;
            }
            lstResponseDetail.Add(responseDetail);
            return responseStatus;
        }

        public ApplicationConstant.ResponseStatus MaTranPheDuyet(DatabaseConstant.Action action,
                                                       ref THONG_TIN_CHUNG objThongTinChung,
                                                       ref List<BS_ResponseDetail> lstResponseDetail,
                                                       ref HT_CNANG obj,
                                                       ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            ApplicationConstant.ResponseStatus responseStatus = new ApplicationConstant.ResponseStatus();
            BS_ResponseDetail responseDetail = new BS_ResponseDetail();
            switch (action)
            {
                case DatabaseConstant.Action.LUU: //Luu
                    responseStatus = new BS_QuanTriHeThong_Action().MaTranPheDuyet_Luu(ref objThongTinChung, ref obj, ref responseMessage, ref responseDetail);
                    break;                
                default:
                    responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                    break;
            }
            lstResponseDetail.Add(responseDetail);
            return responseStatus;
        }
    }
}
