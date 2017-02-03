using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using System.Transactions;
using DataModel.EntityFramework;
using BusinessServices.Utilities.DTO;
using BusinessServices.QuanTriHeThong.DTO;
using DataServices.QuanTriHeThong;
using DataServices.DanhMuc;
using System.Data;

namespace BusinessServices.QuanTriHeThong.Detail
{
    public class BS_PhamVi_Detail
    {
        public ApplicationConstant.ResponseStatus Luu(
            ref PHAM_VI objPhamVi,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail
            )
        {
            bool kq = true;
            try
            {
                DS_HT_PHAM_VI dsPhamVi = new DS_HT_PHAM_VI();
                using (TransactionScope trans = new TransactionScope())
                {
                    // Xóa các dữ liệu phân quyền bị xóa
                    List<HT_PHAM_VI> lstPhamVi = new List<HT_PHAM_VI>();
                    if (objPhamVi.MA_PVI_LOAI.Equals(BusinessConstant.LOAI_PHAM_VI.DON_VI.layGiaTri()))
                    {
                        lstPhamVi = dsPhamVi.GetPhamViDonVi_ByDoiTuong(objPhamVi.MA_DTUONG_LOAI, objPhamVi.MA_DTUONG);
                    }
                    if (objPhamVi.MA_PVI_LOAI.Equals(BusinessConstant.LOAI_PHAM_VI.DIA_LY.layGiaTri()))
                    {
                        lstPhamVi = dsPhamVi.GetPhamViDiaLy_ByDoiTuong(objPhamVi.MA_DTUONG_LOAI, objPhamVi.MA_DTUONG);
                    }
                    foreach (HT_PHAM_VI item in lstPhamVi)
                    {
                        kq = dsPhamVi.Xoa(item);
                        if (!kq)
                        {
                            break;
                        }
                    }

                    // Thêm các dữ liệu phân quyền mới
                    if (kq)
                    {
                        foreach (int item in objPhamVi.ID_PVI)
                        {
                            HT_PHAM_VI htPhamVi = new HT_PHAM_VI();
                            //htPhamVi.ID_DTUONG_LOAI = objPhamVi.ID_DTUONG;
                            htPhamVi.MA_DTUONG_LOAI = objPhamVi.MA_DTUONG_LOAI;
                            htPhamVi.ID_DTUONG = objPhamVi.ID_DTUONG;
                            htPhamVi.MA_DTUONG = objPhamVi.MA_DTUONG;
                            htPhamVi.PHAN_LOAI = objPhamVi.PHAN_LOAI.ElementAt(objPhamVi.ID_PVI.IndexOf(item));
                            htPhamVi.MA_PVI_LOAI = objPhamVi.MA_PVI_LOAI;
                            htPhamVi.ID_PVI = item;
                            htPhamVi.MA_PVI = objPhamVi.MA_PVI.ElementAt(objPhamVi.ID_PVI.IndexOf(item));
                            htPhamVi.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            htPhamVi.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                            htPhamVi.MA_DVI_QLY = objPhamVi.MA_DVI_QLY;
                            htPhamVi.MA_DVI_TAO = objPhamVi.MA_DVI_TAO;
                            htPhamVi.NGAY_NHAP = objPhamVi.NGAY_NHAP;
                            htPhamVi.NGUOI_NHAP = objPhamVi.NGUOI_NHAP;
                            htPhamVi.NGAY_CNHAT = objPhamVi.NGAY_CNHAT;
                            htPhamVi.NGUOI_CNHAT = objPhamVi.NGUOI_CNHAT;
                            kq = dsPhamVi.Them(htPhamVi);

                            if (!kq)
                            {
                                break;
                            }
                        }
                    }

                    if (kq)
                    {
                        trans.Complete();
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Successful.layNgonNgu();
                        bsRetDetail.Detail = responseMessage.layGiaTri();
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

        public ApplicationConstant.ResponseStatus Xoa(
            ref PHAM_VI objPhamVi,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail
            )
        {
            bool kq = true;
            try
            {
                DS_HT_PHAM_VI dsPhamVi = new DS_HT_PHAM_VI();
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (int item in objPhamVi.ID_PVI)
                    {
                        HT_PHAM_VI htPhamVi = new HT_PHAM_VI();
                        htPhamVi.ID_DTUONG_LOAI = objPhamVi.ID_DTUONG;
                        htPhamVi.MA_DTUONG_LOAI = objPhamVi.MA_DTUONG_LOAI;
                        htPhamVi.ID_DTUONG = objPhamVi.ID_DTUONG;
                        htPhamVi.MA_DTUONG = objPhamVi.MA_DTUONG;
                        htPhamVi.PHAN_LOAI = objPhamVi.PHAN_LOAI.ElementAt(objPhamVi.ID_PVI.IndexOf(item));
                        htPhamVi.MA_PVI_LOAI = objPhamVi.MA_PVI_LOAI;
                        htPhamVi.ID_PVI = item;
                        htPhamVi.MA_PVI = objPhamVi.MA_PVI.ElementAt(objPhamVi.ID_PVI.IndexOf(item));
                        htPhamVi.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        htPhamVi.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        htPhamVi.MA_DVI_QLY = objPhamVi.MA_DVI_QLY;
                        htPhamVi.MA_DVI_TAO = objPhamVi.MA_DVI_TAO;
                        htPhamVi.NGAY_NHAP = objPhamVi.NGAY_NHAP;
                        htPhamVi.NGUOI_NHAP = objPhamVi.NGUOI_NHAP;
                        htPhamVi.NGAY_CNHAT = objPhamVi.NGAY_CNHAT;
                        htPhamVi.NGUOI_CNHAT = objPhamVi.NGUOI_CNHAT;
                        kq = dsPhamVi.Xoa(htPhamVi);

                        if (!kq)
                        {
                            break;
                        }
                    }

                    if (kq)
                    {
                        trans.Complete();
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Successful.layNgonNgu();
                        bsRetDetail.Detail = responseMessage.layGiaTri();
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

        public ApplicationConstant.ResponseStatus LayDuLieu(
            ref PHAM_VI objPhamVi,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail
            )
        {
            try
            {
                DS_HT_PHAM_VI dsPhamVi = new DS_HT_PHAM_VI();
                List<HT_PHAM_VI> lstPhamVi = new List<HT_PHAM_VI>();
                if (objPhamVi.MA_PVI_LOAI.Equals(BusinessConstant.LOAI_PHAM_VI.DON_VI.layGiaTri()))
                {
                    lstPhamVi = dsPhamVi.GetPhamViDonVi_ByDoiTuong(objPhamVi.MA_DTUONG_LOAI, objPhamVi.MA_DTUONG);
                }
                if (objPhamVi.MA_PVI_LOAI.Equals(BusinessConstant.LOAI_PHAM_VI.DIA_LY.layGiaTri()))
                {
                    lstPhamVi = dsPhamVi.GetPhamViDiaLy_ByDoiTuong(objPhamVi.MA_DTUONG_LOAI, objPhamVi.MA_DTUONG);
                }
                objPhamVi.ID_PVI = new List<int>();
                objPhamVi.MA_PVI = new List<string>();
                objPhamVi.PHAN_LOAI = new List<string>();
                foreach (HT_PHAM_VI item in lstPhamVi)
                {
                    objPhamVi.ID_PVI.Add(item.ID_PVI.Value);
                    objPhamVi.MA_PVI.Add(item.MA_PVI);
                    objPhamVi.PHAN_LOAI.Add(item.PHAN_LOAI);
                }
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            catch (Exception ex)
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;

                bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                bsRetDetail.Detail = responseMessage.layGiaTri();

                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }

        /// <summary>
        /// Trả về 0 nếu dữ liệu hợp lệ và chuẩn bị dữ liệu không thành công.
        /// Trả về 1 nếu dữ liệu hợp lệ và chuẩn bị dữ liệu thành công.        
        /// </summary>
        /// <param name="objThongTinChung"></param>
        /// <param name="obj"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public int KiemTraVaChuanBiDuLieu(ref THONG_TIN_CHUNG objThongTinChung, ref PHAM_VI objPhamVi, ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            //Khai báo và khởi tạo biến
            responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;

            /*
            #region Lấy lại thông tin và kiểm tra tồn tại
            if (objPhamVi.MA_PVI_LOAI.Equals(BusinessConstant.LOAI_PHAM_VI.DON_VI.layGiaTri()))
            {
                foreach (string item in objPhamVi.MA_PVI)
                {
                    if (!item.IsNullOrEmptyOrSpace())
                    {
                        DM_DON_VI objDonVi = new DS_DM_DON_VI().getDonViByMaDonVi(item);
                        if (objDonVi.IsNullOrEmpty())
                        {
                            responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_KhongTonTai;
                            return 0;
                        }
                        else
                            objPhamVi.ID_PVI[objPhamVi.MA_PVI.IndexOf(item)] = objDonVi.ID;
                    }
                }
            }
            #endregion
            */
            return 1;
        }

        
    }
}
