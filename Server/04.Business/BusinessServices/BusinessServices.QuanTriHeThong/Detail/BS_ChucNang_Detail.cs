using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using BusinessServices.Utilities.DTO;
using System.Transactions;
using DataServices.QuanTriHeThong;
using DataModel.EntityFramework;

namespace BusinessServices.QuanTriHeThong.Detail
{
    public class BS_ChucNang_Detail
    {
        public ApplicationConstant.ResponseStatus LuuMaTranPheDuyet(
            ref THONG_TIN_CHUNG objThongTinChung,
            ref HT_CNANG htCNang,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        { 
            bool kq = true;
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_CNANG dsHtCNang = new DS_HT_CNANG();
                    HT_CNANG obj = dsHtCNang.GetByMa(htCNang.MA_CNANG);
                    obj.MA_TRAN_PHE_DUYET = htCNang.MA_TRAN_PHE_DUYET;
                    kq = dsHtCNang.Sua(obj);                    

                    if (kq)
                    {
                        trans.Complete();
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Successful.layNgonNgu();
                        bsRetDetail.Detail = responseMessage.layGiaTri();
                        htCNang = obj;
                        return ApplicationConstant.ResponseStatus.THANH_CONG;
                    }
                    else
                    {
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                        bsRetDetail.Detail = responseMessage.layGiaTri();
                        return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;

                bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                bsRetDetail.Detail = responseMessage.layGiaTri();

                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }        
        }
    }
}
