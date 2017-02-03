using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using DataServices.QuanTriHeThong;
using BusinessServices.Utilities.DTO;
using Utilities.Common;
using System.Collections;
using System.Transactions;

namespace BusinessServices.QuanTriHeThong
{
    public class BS_PhanQuyen
    {

        /// <summary>
        /// Lấy danh sách Tính năng của chức năng
        /// </summary>
        /// <returns></returns>
        public List<HT_CNANG_TNANG> layCNangTNang()
        {
            return new DS_PhanQuyen().layCNangTNang();
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdChucNang(List<int> lstIdChucNang)
        {
            return new DS_PhanQuyen().layCNangTNangTheoListIdChucNang(lstIdChucNang);
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdMenu(List<int> lstIdMenu)
        {
            return new DS_PhanQuyen().layCNangTNangTheoListIdMenu(lstIdMenu);
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdCNangTNang(List<int> lstIdCNangTNang)
        {
            return new DS_PhanQuyen().layCNangTNangTheoListIdCNangTNang(lstIdCNangTNang);
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyen()
        {
            return new DS_PhanQuyen().layCNangPQuyen();
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyenTheoDoiTuong(string maDoiTuong, string loaiDoiTuong)
        {
            return new DS_PhanQuyen().layCNangPQuyenTheoDoiTuong(maDoiTuong, loaiDoiTuong);
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyenTheoDoiTuongChucNang(string maDoiTuong, string loaiDoiTuong, List<int> lstIdChucNang)
        {
            return new DS_PhanQuyen().layCNangPQuyenTheoDoiTuongChucNang(maDoiTuong, loaiDoiTuong, lstIdChucNang);
        }

        public List<HT_CNANG> layCNang()
        {
            return new DS_PhanQuyen().layCNang();
        }

        public List<HT_CNANG> layCNangTheoListIdChucNang(List<int> lstIdChucNang)
        {
            return new DS_PhanQuyen().layCNangTheoListIdChucNang(lstIdChucNang);
        }

        public List<HT_CNANG> layCNangTheoPhanHe(string maPhanHe)
        {
            return new DS_PhanQuyen().layCNangTheoPhanHe(maPhanHe);
        }

        public List<HT_CNANG> layCNangThietLapAPTheoPhanHe(string maPhanHe)
        {
            return new DS_PhanQuyen().layCNangThietLapAPTheoPhanHe(maPhanHe);
        }

        public List<HT_TNANG> layTNang()
        {
            return new DS_PhanQuyen().layTNang();
        }

        public List<HT_TNANG> layTNangTheoListIdTinhNang(List<int> lstIdTinhNang)
        {
            return new DS_PhanQuyen().layTNangTheoListIdTinhNang(lstIdTinhNang);
        }

        public List<HT_TNANG> layTNangDuocPhanQuyen()
        {
            return new DS_PhanQuyen().layTNangDuocPhanQuyen();
        }

        public List<HT_TNGUYEN> layTNguyen(string maTNguyen, string loaiTNguyen)
        {
            return new DS_PhanQuyen().layTNguyen(maTNguyen, loaiTNguyen);
        }

        public List<HT_TNGUYEN_KTHAC> layDSTNguyenKThac(string maNSD, string loaiNSD, string loaiTNguyen=null)
        {
            return new DS_PhanQuyen().layDSTNguyenKThac(maNSD, loaiNSD, loaiTNguyen);
        }

        public bool luuPhanQuyen(string maDoiTuong, string loaiDoiTuong, ArrayList lstPhanQuyen, string nguoiCapNhat)
        {
            return new DS_PhanQuyen().luuPhanQuyen(maDoiTuong, loaiDoiTuong, lstPhanQuyen, nguoiCapNhat);
        }

        public ApplicationConstant.ResponseStatus luuPhanQuyenChucNang(
            string loaiDoiTuong, 
            int idDoiTuong, 
            string maDoiTuong, 
            string maDonVi,
            string nguoiCapNhat,
            string ngayCapNhat,
            List<HT_CNANG_PQUYEN> dsCNangPQuyenXoa, 
            List<HT_CNANG_TNANG> dsCNangTNangThem,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail
            )
        {
            bool kq = true;
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_CNANG_PQUYEN dsHtCNangPQuyen = new DS_HT_CNANG_PQUYEN();
                    DS_HT_TNGUYEN_KTHAC dsHtTNguyenKThac = new DS_HT_TNGUYEN_KTHAC();
                    DS_HT_MENU dsHtMenu = new DS_HT_MENU();

                    // Xóa các dữ liệu phân quyền bị xóa
                    foreach (HT_CNANG_PQUYEN item in dsCNangPQuyenXoa)
                    {
                        HT_CNANG_PQUYEN obj = dsHtCNangPQuyen.GetById(item.ID);
                        kq = dsHtCNangPQuyen.Xoa(obj);
                        if (!kq)
                        {
                            break;
                        }
                    }

                    // Thêm các dữ liệu phân quyền mới
                    if (kq)
                    {
                        foreach (HT_CNANG_TNANG item in dsCNangTNangThem)
                        {
                            HT_CNANG_PQUYEN htCNangPQuyen = new HT_CNANG_PQUYEN();
                            htCNangPQuyen.ID_DTUONG_LOAI = null;
                            htCNangPQuyen.MA_DTUONG_LOAI = loaiDoiTuong;
                            htCNangPQuyen.ID_DTUONG = idDoiTuong;
                            htCNangPQuyen.MA_DTUONG = maDoiTuong;
                            htCNangPQuyen.ID_CNANG_TNANG = item.ID;
                            htCNangPQuyen.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            htCNangPQuyen.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                            htCNangPQuyen.MA_DVI_QLY = maDonVi;
                            htCNangPQuyen.MA_DVI_TAO = maDonVi;
                            htCNangPQuyen.NGAY_NHAP = ngayCapNhat;
                            htCNangPQuyen.NGUOI_NHAP = nguoiCapNhat;
                            htCNangPQuyen.NGAY_CNHAT = ngayCapNhat;
                            htCNangPQuyen.NGUOI_CNHAT = nguoiCapNhat;

                            kq = dsHtCNangPQuyen.Them(htCNangPQuyen);

                            if (!kq)
                            {
                                break;
                            }
                        }
                    }

                    // Xóa tài nguyên khai thác cũ
                    if (kq)
                    {
                        //kq = dsHtTNguyenKThac.XoaTheoDoiTuong(idDoiTuong, maDoiTuong);
                        kq = dsHtTNguyenKThac.XoaTheoDoiTuong(idDoiTuong, maDoiTuong, loaiDoiTuong);
                    }

                    // Thêm tài nguyên khai thác mới dựa trên danh sách phân quyền mới
                    if (kq)
                    {
                        // Lấy danh sách phân quyền mới của đối tượng
                        List<HT_CNANG_PQUYEN> dsCNangPQuyen = layCNangPQuyenTheoDoiTuong(maDoiTuong, loaiDoiTuong);

                        // Xây dựng danh sách menu trên danh sách phân quyền mới
                        List<int> lstIdCNangTNang = dsCNangPQuyen.Select(e => e.ID_CNANG_TNANG.Value).ToList();
                        List<HT_CNANG_TNANG> lstCNangTNang = layCNangTNangTheoListIdCNangTNang(lstIdCNangTNang);
                        List<int> lstIdCNang = lstCNangTNang.Select(e => e.ID_CNANG).ToList();
                        List<HT_CNANG> lstCNang = layCNangTheoListIdChucNang(lstIdCNang);
                        List<HT_CNANG> lstCNangChoMenu = new List<HT_CNANG>();
                        List<HT_MENU> lstMenu = new List<HT_MENU>();

                        foreach (HT_CNANG item in lstCNang)
                        {
                            DuyetPhanCapChucNang(item, ref lstCNangChoMenu);
                        }
                        List<int> lstIdCNangMenu = lstCNangChoMenu.Select(e => e.ID).ToList();
                        lstMenu = dsHtMenu.GetListByIdChucNang(lstIdCNangMenu);
                        foreach (HT_MENU item in lstMenu)
                        {
                            HT_TNGUYEN_KTHAC htTNguyenKThac = new HT_TNGUYEN_KTHAC();
                            htTNguyenKThac.ID_TNGUYEN_LOAI = 3;
                            htTNguyenKThac.MA_TNGUYEN_LOAI = BusinessConstant.LoaiTaiNguyen.MENU.layGiaTri();
                            htTNguyenKThac.ID_DTUONG_LOAI = 0;
                            htTNguyenKThac.MA_DTUONG_LOAI = loaiDoiTuong;
                            htTNguyenKThac.ID_DTUONG = idDoiTuong;
                            htTNguyenKThac.MA_DTUONG = maDoiTuong;
                            htTNguyenKThac.ID_TNGUYEN = item.ID;
                            htTNguyenKThac.MA_TNGUYEN = item.MA_MENU;
                            htTNguyenKThac.TEN_TNGUYEN = item.TEN_MENU;
                            htTNguyenKThac.GTRI_TNGUYEN = null;
                            htTNguyenKThac.ID_TNGUYEN_CHA = item.ID_MENU_CHA;
                            htTNguyenKThac.KIEU_DLIEU = "String";
                            htTNguyenKThac.NGUON_TAO_DL = DatabaseConstant.NguonTaoDuLieu.NSD.layGiaTri();
                            htTNguyenKThac.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            htTNguyenKThac.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                            htTNguyenKThac.MA_DVI_QLY = maDonVi;
                            htTNguyenKThac.MA_DVI_TAO = maDonVi;
                            htTNguyenKThac.NGAY_NHAP = ngayCapNhat;
                            htTNguyenKThac.NGUOI_NHAP = nguoiCapNhat;
                            htTNguyenKThac.NGAY_CNHAT = ngayCapNhat;
                            htTNguyenKThac.NGUOI_CNHAT = nguoiCapNhat;

                            kq = dsHtTNguyenKThac.Them(htTNguyenKThac);
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

        private void DuyetPhanCapChucNang(HT_CNANG htCNang, ref List<HT_CNANG> lstHtCnang)
        { 
        
            if (htCNang.ID_CNANG_CHA != null)
            {
                lstHtCnang.Add(htCNang);

                bool chkExist = false;
                foreach (HT_CNANG item in lstHtCnang)
                {
                    if (item.ID == htCNang.ID_CNANG_CHA)
                    {
                        chkExist = true;
                        break;
                    }
                }

                HT_CNANG htCNangCha = new DS_HT_CNANG().GetById(htCNang.ID_CNANG_CHA.Value);
                if (!chkExist)
                {                    
                    lstHtCnang.Add(htCNangCha);
                }

                if (htCNangCha.ID_CNANG_CHA != null)
                    DuyetPhanCapChucNang(htCNangCha, ref lstHtCnang);
            }
        }
    }
}
