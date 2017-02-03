using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using DataServices.QuanTriHeThong;
using DataServices.DanhMuc;
using DataServices.Utilities;
using BusinessServices.Utilities.DTO;
using System.Data;
using Utilities.Common;
using System.Transactions;
using BusinessServices.Utilities;

namespace BusinessServices.QuanTriHeThong
{
    /// <summary>
    /// 
    /// </summary>
    public class BS_QuanTriHeThong
    {
        // Nhóm mặc định cho QTTW
        private HT_NHNSD DEF_QTTW = new HT_NHNSD();
        // Nhóm mặc định cho QTDV
        private HT_NHNSD DEF_QTDV = new HT_NHNSD();

        #region Lấy chức năng khi đăng nhập

        /// <summary>
        /// Lấy tài nguyên (chức năng)
        /// </summary>
        /// <param name="maDangNhap"></param>
        /// <param name="maDonVi"></param>
        /// <returns></returns>
        public DataSet layDanhSachChucNangTheoUser(string maDangNhap, string maDonVi)
        {
            DS_PhanQuyen layTaiNguyenService = new DS_PhanQuyen();
            DataSet ds = layTaiNguyenService.LayDanhSachChucNangTheoUser(maDangNhap, maDonVi);
            return ds;
        }


        public DataSet layDanhSachChucNangWebTheoUser(string maDangNhap, string maDonVi)
        {
            DS_PhanQuyen layTaiNguyenService = new DS_PhanQuyen();
            DataSet ds = layTaiNguyenService.LayDanhSachChucNangWebTheoUser(maDangNhap, maDonVi);
            return ds;
        }

        public DataSet LayMenuTheoUser(string maDangNhap, string maDonVi)
        {
            DS_PhanQuyen layTaiNguyenService = new DS_PhanQuyen();
            DataSet ds = layTaiNguyenService.LayMenuTheoUser(maDangNhap, maDonVi);
            return ds;
        }

        /// <summary>
        /// Lấy tài nguyên (chức năng)
        /// </summary>
        /// <param name="maDangNhap"></param>
        /// <param name="maDonVi"></param>
        /// <returns></returns>
        //public List<SP_SELECT_LayDSachCNangTNang_Result> layDanhSachChucNangTheoUserEF(string maDangNhap, string maDonVi)
        //{
        //    HtPhanQuyen layTaiNguyenService = new HtPhanQuyen();
        //    List<SP_SELECT_LayDSachCNangTNang_Result> lstDSachCNang = layTaiNguyenService.LayDanhSachChucNangTheoUserEF(maDangNhap, maDonVi);
        //    return lstDSachCNang;
        //}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maChucNang"></param>
        /// <returns></returns>
        public HT_CNANG LayChucNangTheoMa(string maChucNang)
        {
            DS_CNANG dsService = new DS_CNANG();
            HT_CNANG result = dsService.LayChucNangTheoMa(maChucNang);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maChucNangCha"></param>
        /// <returns></returns>
        public List<HT_CNANG> LayChucNangTheoMaCha(string maChucNangCha)
        {
            DS_CNANG dsService = new DS_CNANG();
            List<HT_CNANG> result = dsService.LayChucNangTheoMaCha(maChucNangCha);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maDangNhap"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public List<HT_CNANG> LayChucNangTheoQuyen(string maDangNhap, string passWord)
        {
            DS_QuanTriHeThong dsService = new DS_QuanTriHeThong();
            List<HT_CNANG> result = dsService.LayChucNangTheoQuyen(maDangNhap, passWord);
            return result;
        }

        /// <summary>
        /// Lấy thông tin ngày làm việc theo đơn vị
        /// </summary>
        public HT_NGAY_LVIEC getNgayLamViecTheoDonVi(string maDonVi)
        {
            return new DS_HT_NGAY_LVIEC().getNgayLamViecTheoDonVi(maDonVi);
        }


        public List<DM_DON_VI> layDanhSachDonVi()
        {
            List<DM_DON_VI> hoiSo = null;
            List<DM_DON_VI> lstChiNhanh = null;

            lstChiNhanh = new DS_DM_DON_VI().getListDonViByLoaiDonVi(DatabaseConstant.ToChucDonVi.CNH.getValue());
            hoiSo = new DS_DM_DON_VI().getListDonViByLoaiDonVi(DatabaseConstant.ToChucDonVi.HSO.getValue());

            if (lstChiNhanh != null && hoiSo != null)
                lstChiNhanh.Add(hoiSo[0]);

            return lstChiNhanh;
        }

        public HT_NSD layThongTinCaNhan(int idNguoiDung, string tenDangNhap, string maDonVi)
        {
            return new DS_HT_NSD().GetById(idNguoiDung);
        }

        public List<HT_NSD> layNSD(BusinessConstant.LoaiNguoiSuDung loaiNguoiSuDung, string maDonVi)
        {
            // Nếu là SA hoặc QTTW thì lấy toàn bộ, trừ HTH
            if (loaiNguoiSuDung == BusinessConstant.LoaiNguoiSuDung.CAP_SA ||
                loaiNguoiSuDung == BusinessConstant.LoaiNguoiSuDung.CAP_QTTW)
            {
                return new DS_HT_NSD().GetAll();
            }
            // Nếu là QTDV, thì lấy theo đơn vị mình
            else if (loaiNguoiSuDung == BusinessConstant.LoaiNguoiSuDung.CAP_QTDV)
            {
                return new DS_HT_NSD().GetListByDonVi(maDonVi);
            }
            // Còn lại, thì lấy theo đơn vị mình
            else
            {
                return new DS_HT_NSD().GetListByDonVi(maDonVi);
            }
        }

        public List<HT_NHNSD> layNhomNSD(BusinessConstant.LoaiNguoiSuDung loaiNguoiSuDung, string maDonVi)
        {
            // Nếu là SA hoặc QTTW thì lấy toàn bộ, trừ HTH
            if (loaiNguoiSuDung == BusinessConstant.LoaiNguoiSuDung.CAP_SA ||
                loaiNguoiSuDung == BusinessConstant.LoaiNguoiSuDung.CAP_QTTW)
            {
                return new DS_HT_NHNSD().GetAll();
            }
            // Nếu là QTDV, thì lấy theo đơn vị mình
            else if (loaiNguoiSuDung == BusinessConstant.LoaiNguoiSuDung.CAP_QTDV)
            {
                return new DS_HT_NHNSD().GetListByDonVi(maDonVi);
            }
            // Còn lại, thì lấy theo đơn vị mình
            else
            {
                return new DS_HT_NHNSD().GetListByDonVi(maDonVi);
            }
        }

        public List<HT_NSD> layNSDTheoNhom(int id_NHNSD)
        {
            // Không lấy người dùng hệ thống
            string nguonHeThong = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
            List<int> idNSD = new DS_HT_NHNSD_NSD().GetByIdNHNSD(id_NHNSD).Where(e => e.NGUON_TAO_DL != nguonHeThong).Select(e => e.ID_NSD).ToList();
            return new DS_HT_NSD().GetByListId(idNSD);
        }

        public List<HT_NHNSD> layNhomTheoNSD(int id_NSD)
        {
            // Không lấy nhóm hệ thống
            string nguonHeThong = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
            List<int> idNHNSD = new DS_HT_NHNSD_NSD().GetByIdNSD(id_NSD).Where(e => e.NGUON_TAO_DL != nguonHeThong).Select(e => e.ID_NHNSD).ToList();
            return new DS_HT_NHNSD().GetByListId(idNHNSD);
        }

        public List<BS_PhamVi> layPhamViTheoIdNSDVaMaLoaiPhamVi(int id_NSD, string maLoaiPhamVi)
        {
            List<HT_NSD_PVI> listHtNsdPvi = new DS_HT_NSD_PVI().GetByIdNSDAndMaLoaiPhamVi(id_NSD, maLoaiPhamVi);
            if (LObject.IsNullOrEmpty(listHtNsdPvi))
            {
                return null;
            }
            else
            {
                List<BS_PhamVi> listBsPhamVi = new List<BS_PhamVi>();
                Entities entities = ContextFactory.GetInstance();
                string listStrMaPvi = "";
                List<string> listMaPvi = new List<string>();

                if (listHtNsdPvi != null && listHtNsdPvi.Count > 0)
                {
                    listMaPvi = listHtNsdPvi.Select(e => e.MA_PVI).Distinct().ToList();
                    foreach (string item in listMaPvi)
                    {
                        listStrMaPvi = listStrMaPvi + "'" + item + "', ";
                    }
                    listStrMaPvi = listStrMaPvi.Remove(listStrMaPvi.Length - 2);

                    if (maLoaiPhamVi.Equals(BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi()))
                    {
                        int ddLoaiPvi = BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layIdPhamVi();
                        string maLoaiPvi = BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi();
                        listBsPhamVi = (from PV in entities.DM_DON_VI
                                        where listStrMaPvi.Contains("'" + PV.MA_DVI + "'")
                                        select new BS_PhamVi()
                                        {
                                            IdLoaiPvi = ddLoaiPvi,
                                            MaLoaiPvi = maLoaiPvi,
                                            IdPvi = PV.ID,
                                            MaPvi = PV.MA_DVI,
                                            TenPvi = PV.TEN_GDICH,
                                            PhuongPhapHachToan = PV.MA_HACH_TOAN
                                        }).ToList();
                    }
                }
                
                return listBsPhamVi;
            }
        }

        public List<HT_TRUY_CAP> LayTruyCapTheoNSD(int id_NSD)
        {
            string sNSD = BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri();
            return new DS_HT_TRUY_CAP().GetByDoiTuong(id_NSD, sNSD);
        }

        public List<HT_TRUY_CAP> LayTruyCapTheoNhomNSD(int id_NHNSD)
        {
            string sNhomNSD = BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri();
            return new DS_HT_TRUY_CAP().GetByDoiTuong(id_NHNSD, sNhomNSD);
        }

        public ApplicationConstant.ResponseStatus ThemNSD(ref HT_NSD obj, 
            List<int> lstIdNHNSD, 
            List<BS_PhamVi> lstPhamVi,
            List<HT_TRUY_CAP> lstTruyCap,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        {
            bool kq = true;

            bsRetDetail.Object = obj.MA_DANG_NHAP + " (" + obj.TEN_DAY_DU + ")";
            bsRetDetail.Operation = DatabaseConstant.Action.THEM.layNgonNgu();
            try
            {
                // Kiểm tra thông tin người dùng trước khi thêm
                // Hiện tại mã đăng nhập trong toàn hệ thống là duy nhất, không phân biệt đơn vị
                if (new DS_HT_NSD().GetByMa(obj.MA_DANG_NHAP) != null)
                {
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_DaTonTai;

                    bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                    bsRetDetail.Detail = responseMessage.layGiaTri();

                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }

                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_NSD dsNsd = new DS_HT_NSD();
                    DS_HT_NHNSD dsNhNsd = new DS_HT_NHNSD();
                    DS_HT_NHNSD_NSD dsNhNsd_Nsd = new DS_HT_NHNSD_NSD();
                    DS_HT_NSD_PVI dsNsdPvi = new DS_HT_NSD_PVI();

                    List<HT_NHNSD> lstNHNSD = dsNhNsd.GetByListId(lstIdNHNSD);
                    lstNHNSD = lstNHNSD != null ? lstNHNSD : new List<HT_NHNSD>();

                    HT_NSD htNsd = dsNsd.Them(obj);

                    if (htNsd != null)
                    {
                        // Nếu người dùng là CAP_QTTW >> thêm nhóm mặc định
                        if (htNsd.PHAN_LOAI_NSD.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                        {
                            DEF_QTTW.ID = BusinessConstant.NhomNguoiSuDung.DEF_QTTW.layIdNhomNguoiSuDung();
                            DEF_QTTW.MA_NHNSD = BusinessConstant.NhomNguoiSuDung.DEF_QTTW.layMaNhomNguoiSuDung();
                            DEF_QTTW.NGUON_TAO_DL = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                            lstNHNSD.Add(DEF_QTTW);
                        }

                        // Nếu người dùng là CAP_QTDV >> thêm nhóm mặc định
                        if (htNsd.PHAN_LOAI_NSD.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                        {
                            DEF_QTDV.ID = BusinessConstant.NhomNguoiSuDung.DEF_QTDV.layIdNhomNguoiSuDung();
                            DEF_QTDV.MA_NHNSD = BusinessConstant.NhomNguoiSuDung.DEF_QTDV.layMaNhomNguoiSuDung();
                            DEF_QTDV.NGUON_TAO_DL = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                            lstNHNSD.Add(DEF_QTDV);
                        }

                        // Ghi thông tin nhóm cho người dùng
                        foreach (HT_NHNSD n in lstNHNSD)
                        {
                            HT_NHNSD_NSD objNHNSD_NSD = new HT_NHNSD_NSD();
                            objNHNSD_NSD.ID_NHNSD = n.ID;
                            objNHNSD_NSD.ID_NSD = obj.ID;
                            objNHNSD_NSD.MA_DVI_QLY = obj.MA_DVI_QLY;
                            objNHNSD_NSD.MA_DVI_TAO = obj.MA_DVI_TAO;
                            objNHNSD_NSD.MA_NHNSD = n.MA_NHNSD;
                            objNHNSD_NSD.MA_NSD = obj.MA_NSD;

                            objNHNSD_NSD.NGUON_TAO_DL = n.NGUON_TAO_DL;
                            objNHNSD_NSD.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objNHNSD_NSD.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                            objNHNSD_NSD.NGAY_CNHAT = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGAY_NHAP = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                            objNHNSD_NSD.NGUOI_NHAP = obj.NGUOI_CNHAT;

                            HT_NHNSD_NSD nhomNsd_Nsd = dsNhNsd_Nsd.Them(objNHNSD_NSD);

                            if (nhomNsd_Nsd == null)
                            {
                                kq = false;
                                break;
                            }
                            else
                            {
                                kq = true;
                            }
                        }

                        // Ghi thông tin phạm vi
                        foreach (BS_PhamVi item in lstPhamVi)
                        {
                            HT_NSD_PVI objPhamVi = new HT_NSD_PVI();
                            objPhamVi.ID_NSD = obj.ID;
                            objPhamVi.MA_NSD = obj.MA_NSD;
                            objPhamVi.ID_PVI_LOAI = item.IdLoaiPvi;
                            objPhamVi.MA_PVI_LOAI = item.MaLoaiPvi;
                            objPhamVi.ID_PVI = item.IdPvi;
                            objPhamVi.MA_PVI = item.MaPvi;

                            objPhamVi.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objPhamVi.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                            objPhamVi.MA_DVI_QLY = obj.MA_DVI_QLY;
                            objPhamVi.MA_DVI_TAO = obj.MA_DVI_TAO;
                            objPhamVi.NGAY_CNHAT = obj.NGAY_CNHAT;
                            objPhamVi.NGAY_NHAP = obj.NGAY_CNHAT;
                            objPhamVi.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                            objPhamVi.NGUOI_NHAP = obj.NGUOI_CNHAT;

                            HT_NSD_PVI htNsdPvi = dsNsdPvi.Them(objPhamVi);

                            if (htNsdPvi == null)
                            {
                                kq = false;
                                break;
                            }
                            else
                            {
                                kq = true;
                            }
                        }

                        //Ghi thông tin hạn chế truy cập (MAC và IP)
                        if (kq == true)
                        {
                            if (lstTruyCap != null && lstTruyCap.Count > 0)
                            {
                                //Gán ID người sử dụng
                                for (int i = 0; i < lstTruyCap.Count; i++)
                                {
                                    lstTruyCap[i].ID_DTUONG = htNsd.ID;
                                }

                                foreach (HT_TRUY_CAP objTruyCap in lstTruyCap)
                                {
                                    if (new DS_HT_TRUY_CAP().Them(objTruyCap) == null)
                                    {
                                        kq = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        kq = false;
                    }

                    if (kq == true)
                    {
                        trans.Complete();

                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                        obj = htNsd;

                        bsRetDetail.Id = htNsd.ID;
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
            catch (System.Exception ex)
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;

                bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                bsRetDetail.Detail = responseMessage.layGiaTri();

                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public ApplicationConstant.ResponseStatus SuaNSD(ref HT_NSD obj,
            List<int> lstIdNHNSD,
            List<BS_PhamVi> lstPhamVi,
            List<HT_TRUY_CAP> lstTruyCap,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        {
            bool kq = true;
            bsRetDetail.Id = obj.ID;
            bsRetDetail.Object = obj.MA_DANG_NHAP + " (" + obj.TEN_DAY_DU + ")";
            bsRetDetail.Operation = DatabaseConstant.Action.SUA.layNgonNgu();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_NSD dsNsd = new DS_HT_NSD();
                    DS_HT_NHNSD dsNhNsd = new DS_HT_NHNSD();
                    DS_HT_NHNSD_NSD dsNhNsd_Nsd = new DS_HT_NHNSD_NSD();
                    DS_HT_NSD_PVI dsNsdPvi = new DS_HT_NSD_PVI();
                    HT_NSD htNsd = null;

                    // Sửa thông tin người dùng
                    if (kq == true)
                    {
                        htNsd = dsNsd.Sua(obj);
                        kq = htNsd != null ? true : false;
                    }

                    // Xóa thông tin các nhóm cũ của người dùng
                    if (kq == true)
                    {
                        kq = dsNhNsd_Nsd.XoaTheoID_NHNSD(obj.ID, BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG);
                    }

                    // Xóa thông tin các phạm vi cũ của người dùng
                    // Trước mắt xóa phạm vi phòng giao dịch
                    if (kq == true)
                    {
                        kq = dsNsdPvi.XoaTheoID_NSD_MaLoaiPhamVi(obj.ID, BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi());
                    }

                    // Lấy danh sách các nhóm mới của người dùng
                    if (kq == true)
                    {
                        List<HT_NHNSD> lstNHNSD = dsNhNsd.GetByListId(lstIdNHNSD);
                        lstNHNSD = lstNHNSD != null ? lstNHNSD : new List<HT_NHNSD>();

                        // Nếu người dùng là CAP_QTTW >> thêm nhóm mặc định
                        if (htNsd.PHAN_LOAI_NSD.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                        {
                            DEF_QTTW.ID = BusinessConstant.NhomNguoiSuDung.DEF_QTTW.layIdNhomNguoiSuDung();
                            DEF_QTTW.MA_NHNSD = BusinessConstant.NhomNguoiSuDung.DEF_QTTW.layMaNhomNguoiSuDung();
                            DEF_QTTW.NGUON_TAO_DL = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                            lstNHNSD.Add(DEF_QTTW);
                        }

                        // Nếu người dùng là CAP_QTDV >> thêm nhóm mặc định
                        if (htNsd.PHAN_LOAI_NSD.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                        {
                            DEF_QTDV.ID = BusinessConstant.NhomNguoiSuDung.DEF_QTDV.layIdNhomNguoiSuDung();
                            DEF_QTDV.MA_NHNSD = BusinessConstant.NhomNguoiSuDung.DEF_QTDV.layMaNhomNguoiSuDung();
                            DEF_QTDV.NGUON_TAO_DL = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                            lstNHNSD.Add(DEF_QTDV);
                        }

                        // Ghi thông tin các nhóm mới                    
                        foreach (HT_NHNSD n in lstNHNSD)
                        {
                            HT_NHNSD_NSD objNHNSD_NSD = new HT_NHNSD_NSD();
                            objNHNSD_NSD.ID_NHNSD = n.ID;
                            objNHNSD_NSD.ID_NSD = obj.ID;
                            objNHNSD_NSD.MA_DVI_QLY = obj.MA_DVI_QLY;
                            objNHNSD_NSD.MA_DVI_TAO = obj.MA_DVI_TAO;
                            objNHNSD_NSD.MA_NHNSD = n.MA_NHNSD;
                            objNHNSD_NSD.MA_NSD = obj.MA_NSD;

                            objNHNSD_NSD.NGUON_TAO_DL = n.NGUON_TAO_DL;
                            objNHNSD_NSD.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objNHNSD_NSD.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                            objNHNSD_NSD.NGAY_CNHAT = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGAY_NHAP = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                            objNHNSD_NSD.NGUOI_NHAP = obj.NGUOI_CNHAT;
                            

                            HT_NHNSD_NSD nhomNsd_Nsd = dsNhNsd_Nsd.Them(objNHNSD_NSD);

                            if (nhomNsd_Nsd == null)
                            {
                                kq = false;
                                break;
                            }
                            else
                            {
                                kq = true;
                            }
                        }

                        // Ghi thông tin phạm vi mới
                        foreach (BS_PhamVi item in lstPhamVi)
                        {
                            HT_NSD_PVI objPhamVi = new HT_NSD_PVI();
                            objPhamVi.ID_NSD = obj.ID;
                            objPhamVi.MA_NSD = obj.MA_NSD;
                            objPhamVi.ID_PVI_LOAI = item.IdLoaiPvi;
                            objPhamVi.MA_PVI_LOAI = item.MaLoaiPvi;
                            objPhamVi.ID_PVI = item.IdPvi;
                            objPhamVi.MA_PVI = item.MaPvi;

                            objPhamVi.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objPhamVi.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                            objPhamVi.MA_DVI_QLY = obj.MA_DVI_QLY;
                            objPhamVi.MA_DVI_TAO = obj.MA_DVI_TAO;
                            objPhamVi.NGAY_CNHAT = obj.NGAY_CNHAT;
                            objPhamVi.NGAY_NHAP = obj.NGAY_CNHAT;
                            objPhamVi.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                            objPhamVi.NGUOI_NHAP = obj.NGUOI_CNHAT;

                            HT_NSD_PVI htNsdPvi = dsNsdPvi.Them(objPhamVi);

                            if (htNsdPvi == null)
                            {
                                kq = false;
                                break;
                            }
                            else
                            {
                                kq = true;
                            }
                        }

                        //Thông tin hạn chế truy cập (MAC và IP)
                        if (kq == true)
                        {
                            //Xóa cũ
                            List<HT_TRUY_CAP> lstTruyCapCu = new DS_HT_TRUY_CAP().GetByDoiTuong(htNsd.ID, BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri());
                            if (lstTruyCapCu != null && lstTruyCapCu.Count > 0)
                            {
                                kq = new DS_HT_TRUY_CAP().XoaTheoList(lstTruyCapCu);
                            }

                            //Nếu xóa bản ghi cũ thành công thì thêm mới
                            if (kq == true)
                            {
                                foreach (HT_TRUY_CAP objTruyCap in lstTruyCap)
                                {
                                    if (new DS_HT_TRUY_CAP().Them(objTruyCap) == null)
                                    {
                                        kq = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (kq == true)
                    {
                        trans.Complete();

                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                        obj = htNsd;

                        bsRetDetail.Id = htNsd.ID;
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

        private ApplicationConstant.ResponseStatus XoaNSD(int id,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        {
            bool kq = true;
            
            bsRetDetail.Id = id;
            bsRetDetail.Operation = DatabaseConstant.Action.XOA.layNgonNgu();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_NSD dsNsd = new DS_HT_NSD();
                    DS_HT_NHNSD_NSD dsNhNsd_Nsd = new DS_HT_NHNSD_NSD();
                    DS_HT_TNGUYEN_KTHAC dsTNguyenKthac = new DS_HT_TNGUYEN_KTHAC();
                    DS_HT_CNANG_PQUYEN dsCNangPquyen = new DS_HT_CNANG_PQUYEN();
                    HT_NSD htNsd = null;

                    // Lấy thông tin người dùng
                    htNsd = dsNsd.GetById(id);
                    kq = htNsd != null ? true : false;

                    // Nếu không tồn tại dữ liệu
                    if (kq == false)
                    {
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_KhongTonTai;

                        bsRetDetail.Object = id.ToString();
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                        bsRetDetail.Detail = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongTonTaiDuLieu.layGiaTri();

                        return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    }

                    bsRetDetail.Object = htNsd.MA_DANG_NHAP + " (" + htNsd.TEN_DAY_DU + ")";

                    // Kiểm tra yêu cầu nghiệp vụ trước khi thực hiện

                    // Kiểm tra ràng buộc dữ liệu trước khi thực hiện
                    string tableConstraint = new DS_DataConstraint().checkDataConstraint(DatabaseConstant.Table.HT_NSD.getValue(), "MA_NSD" , htNsd.MA_NSD);
                    kq = tableConstraint != null ? false : true;

                    if (kq == false)
                    {
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_RangBuocDuLieu;

                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                        bsRetDetail.Detail = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_RangBuocDuLieu.layGiaTri() + "#" + tableConstraint;

                        return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    }

                    // Nếu không có ràng buộc, bắt đầu thực hiện xóa dữ liệu
                    // Xóa dữ liệu tài nguyên khai thác
                    else if (kq == true)
                    {
                        kq = dsTNguyenKthac.XoaTheoDoiTuong(id, htNsd.MA_NSD, BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri());
                    }

                    // Xóa dữ liệu phân quyền
                    if (kq == true)
                    {
                        kq = dsCNangPquyen.XoaTheoDoiTuong(id, htNsd.MA_NSD, BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri());
                    }

                    // Xóa thông tin các ràng buộc nhóm-người dùng
                    if (kq == true)
                    {
                        kq = dsNhNsd_Nsd.XoaTheoID_NHNSD(id, BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG);
                    }

                    // Xóa thông tin người dùng
                    if (kq == true)
                    {                    
                        kq = dsNsd.Xoa(htNsd);
                    }

                    //Xóa thông tin hạn chế truy cập (MAC or IP)
                    if (kq == true)
                    {
                        List<HT_TRUY_CAP> lstTruyCap = new DS_HT_TRUY_CAP().GetByDoiTuong(htNsd.ID, BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri());

                        if (lstTruyCap != null && lstTruyCap.Count > 0)
                        {
                            kq = new DS_HT_TRUY_CAP().XoaTheoList(lstTruyCap);
                        }
                    }

                    if (kq == true)
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

        public ApplicationConstant.ResponseStatus XoaListNSDTheoId(List<int> listID,            
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref List<BS_ResponseDetail> listResponseDetail)
        {
            bool tempVal = true;

            foreach (int inf in listID)
            {
                BS_ResponseDetail bsRetDetail = new BS_ResponseDetail();
                bsRetDetail.Id = inf;
                bsRetDetail.Operation = DatabaseConstant.Action.XOA.layNgonNgu();

                ApplicationConstant.ResponseStatus ret = XoaNSD(inf, ref responseMessage, ref bsRetDetail);
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    listResponseDetail.Add(bsRetDetail);
                }
                else
                {
                    listResponseDetail.Add(bsRetDetail);
                    tempVal = false;
                }
            }

            if (tempVal)
            {
                //responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            else
            {
                //responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }
        
        public ApplicationConstant.ResponseStatus ThemNHNSD(ref HT_NHNSD obj, 
            List<int> lstIdNSD,
            List<HT_TRUY_CAP> lstTruyCap,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        {
            bool kq = true;

            bsRetDetail.Object = obj.MA_NHNSD + " (" + obj.TEN_NHNSD + ")";
            bsRetDetail.Operation = DatabaseConstant.Action.THEM.layNgonNgu();
            try
            {
                // Kiểm tra thông tin nhóm người dùng trước khi thêm
                // Hiện tại mã nhóm người dùng là duy nhất trong một đơn vị
                if (new DS_HT_NHNSD().GetByMa(obj.MA_NHNSD) != null)
                {
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NhomNguoiDung_DaTonTai;

                    bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                    bsRetDetail.Detail = responseMessage.layGiaTri();

                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }

                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_NSD dsNsd = new DS_HT_NSD();
                    DS_HT_NHNSD dsNhNsd = new DS_HT_NHNSD();
                    DS_HT_NHNSD_NSD dsNhNsd_Nsd = new DS_HT_NHNSD_NSD();

                    List<HT_NSD> lstNSD = dsNsd.GetByListId(lstIdNSD);
                    lstNSD = lstNSD != null ? lstNSD : new List<HT_NSD>();

                    HT_NHNSD htNhNsd = dsNhNsd.Them(obj);

                    if (htNhNsd != null)
                    {
                        foreach (HT_NSD n in lstNSD)
                        {
                            HT_NHNSD_NSD objNHNSD_NSD = new HT_NHNSD_NSD();
                            objNHNSD_NSD.ID_NHNSD = obj.ID;
                            objNHNSD_NSD.ID_NSD = n.ID;
                            objNHNSD_NSD.MA_DVI_QLY = obj.MA_DVI_QLY;
                            objNHNSD_NSD.MA_DVI_TAO = obj.MA_DVI_TAO;
                            objNHNSD_NSD.MA_NHNSD = obj.MA_NHNSD;
                            objNHNSD_NSD.MA_NSD = n.MA_NSD;
                            objNHNSD_NSD.NGAY_CNHAT = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGAY_NHAP = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                            objNHNSD_NSD.NGUOI_NHAP = obj.NGUOI_CNHAT;

                            objNHNSD_NSD.NGUON_TAO_DL = obj.NGUON_TAO_DL;
                            objNHNSD_NSD.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objNHNSD_NSD.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                            HT_NHNSD_NSD nhomNsd_Nsd = dsNhNsd_Nsd.Them(objNHNSD_NSD);

                            if (nhomNsd_Nsd == null)
                            {
                                kq = false;
                                break;
                            }
                            else
                            {
                                kq = true;
                            }
                        }

                        //Ghi thông tin hạn chế truy cập (MAC và IP)
                        if (kq == true)
                        {
                            if (lstTruyCap != null && lstTruyCap.Count > 0)
                            {
                                //Gán ID người sử dụng
                                for (int i = 0; i < lstTruyCap.Count; i++)
                                {
                                    lstTruyCap[i].ID_DTUONG = htNhNsd.ID;
                                }

                                foreach (HT_TRUY_CAP objTruyCap in lstTruyCap)
                                {
                                    if (new DS_HT_TRUY_CAP().Them(objTruyCap) == null)
                                    {
                                        kq = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        kq = false;
                    }

                    if (kq == true)
                    {
                        trans.Complete();

                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                        obj = htNhNsd;

                        bsRetDetail.Id = htNhNsd.ID;
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
            finally
            { }
        }

        public ApplicationConstant.ResponseStatus SuaNHNSD(ref HT_NHNSD obj, 
            List<int> lstIdNSD,
            List<HT_TRUY_CAP> lstTruyCap,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        {
            bool kq = true;
            bsRetDetail.Id = obj.ID;
            bsRetDetail.Object = obj.MA_NHNSD + " (" + obj.TEN_NHNSD + ")";
            bsRetDetail.Operation = DatabaseConstant.Action.SUA.layNgonNgu();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_NSD dsNsd = new DS_HT_NSD();
                    DS_HT_NHNSD dsNhNsd = new DS_HT_NHNSD();
                    DS_HT_NHNSD_NSD dsNhNsd_Nsd = new DS_HT_NHNSD_NSD();
                    HT_NHNSD htNhNsd = null;

                    // Sửa thông tin người dùng
                    if (kq == true)
                    {
                        htNhNsd = dsNhNsd.Sua(obj);
                        kq = htNhNsd != null ? true : false;
                    }

                    // Xóa thông tin các nhóm cũ của người dùng
                    if (kq == true)
                    {
                        kq = dsNhNsd_Nsd.XoaTheoID_NHNSD(obj.ID, BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG);
                    }

                    // Lấy danh sách các nhóm mới của người dùng
                    if (kq == true)
                    {
                        List<HT_NSD> lstNSD = dsNsd.GetByListId(lstIdNSD);
                        lstNSD = lstNSD != null ? lstNSD : new List<HT_NSD>();

                        foreach (HT_NSD n in lstNSD)
                        {
                            HT_NHNSD_NSD objNHNSD_NSD = new HT_NHNSD_NSD();
                            objNHNSD_NSD.ID_NHNSD = obj.ID;
                            objNHNSD_NSD.ID_NSD = n.ID;
                            objNHNSD_NSD.MA_DVI_QLY = obj.MA_DVI_QLY;
                            objNHNSD_NSD.MA_DVI_TAO = obj.MA_DVI_TAO;
                            objNHNSD_NSD.MA_NHNSD = obj.MA_NHNSD;
                            objNHNSD_NSD.MA_NSD = n.MA_NSD;
                            objNHNSD_NSD.NGAY_CNHAT = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGAY_NHAP = obj.NGAY_CNHAT;
                            objNHNSD_NSD.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                            objNHNSD_NSD.NGUOI_NHAP = obj.NGUOI_CNHAT;
                            objNHNSD_NSD.NGUON_TAO_DL = obj.NGUON_TAO_DL;
                            objNHNSD_NSD.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objNHNSD_NSD.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                            HT_NHNSD_NSD nhomNsd_Nsd = dsNhNsd_Nsd.Them(objNHNSD_NSD);

                            if (nhomNsd_Nsd == null)
                            {
                                kq = false;
                                break;
                            }
                            else
                            {
                                kq = true;
                            }
                        }
                    }

                    //Thông tin hạn chế truy cập (MAC và IP)
                    if (kq == true)
                    {
                        //Xóa cũ
                        List<HT_TRUY_CAP> lstTruyCapCu = new DS_HT_TRUY_CAP().GetByDoiTuong(htNhNsd.ID, BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri());
                        if (lstTruyCapCu != null && lstTruyCapCu.Count > 0)
                        {
                            kq = new DS_HT_TRUY_CAP().XoaTheoList(lstTruyCapCu);
                        }

                        //Nếu xóa bản ghi cũ thành công thì thêm mới
                        if (kq == true)
                        {
                            foreach (HT_TRUY_CAP objTruyCap in lstTruyCap)
                            {
                                if (new DS_HT_TRUY_CAP().Them(objTruyCap) == null)
                                {
                                    kq = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (kq == true)
                    {
                        trans.Complete();

                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                        obj = htNhNsd;

                        bsRetDetail.Id = htNhNsd.ID;
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

        private ApplicationConstant.ResponseStatus XoaNHNSD(int id,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        {
            bool kq = true;
            
            bsRetDetail.Id = id;
            bsRetDetail.Operation = DatabaseConstant.Action.XOA.layNgonNgu();

            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    DS_HT_NHNSD dsNhNsd = new DS_HT_NHNSD();
                    DS_HT_NHNSD_NSD dsNhNsd_Nsd = new DS_HT_NHNSD_NSD();
                    DS_HT_TNGUYEN_KTHAC dsTNguyenKthac = new DS_HT_TNGUYEN_KTHAC();
                    DS_HT_CNANG_PQUYEN dsCNangPquyen = new DS_HT_CNANG_PQUYEN();
                    HT_NHNSD htNhNsd = null;

                    // Lấy thông tin nhóm người dùng
                    htNhNsd = dsNhNsd.GetById(id);
                    kq = htNhNsd != null ? true : false;

                    // Nếu không tồn tại dữ liệu
                    if (kq == false)
                    {
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_KhongTonTai;

                        bsRetDetail.Object = id.ToString();
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                        bsRetDetail.Detail = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongTonTaiDuLieu.layGiaTri();

                        return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    }

                    // Kiểm tra yêu cầu nghiệp vụ trước khi thực hiện

                    // Kiểm tra ràng buộc dữ liệu trước khi thực hiện
                    string tableConstraint = new DS_DataConstraint().checkDataConstraint(DatabaseConstant.Table.HT_NHNSD.getValue(), "MA_NHNSD", htNhNsd.MA_NHNSD);
                    kq = tableConstraint != null ? false : true;

                    if (kq == false)
                    {
                        bsRetDetail.Object = htNhNsd.MA_NHNSD + " (" + htNhNsd.TEN_NHNSD + ")";
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Failed.layNgonNgu();
                        bsRetDetail.Detail = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_RangBuocDuLieu.layGiaTri() + "#" + tableConstraint;

                        return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    }

                    // Nếu không có ràng buộc, bắt đầu thực hiện xóa dữ liệu
                    // Xóa dữ liệu tài nguyên khai thác
                    else if (kq == true)
                    {
                        kq = dsTNguyenKthac.XoaTheoDoiTuong(id, htNhNsd.MA_NHNSD, BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri());
                    }

                    // Xóa dữ liệu phân quyền
                    if (kq == true)
                    {
                        kq = dsCNangPquyen.XoaTheoDoiTuong(id, htNhNsd.MA_NHNSD, BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri());
                    }

                    // Xóa thông tin các ràng buộc nhóm-người dùng
                    if (kq == true)
                    {
                        kq = dsNhNsd_Nsd.XoaTheoID_NHNSD(id, BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG);
                    }

                    //Xóa thông tin hạn chế truy cập (MAC or IP)
                    if (kq == true)
                    {
                        List<HT_TRUY_CAP> lstTruyCap = new DS_HT_TRUY_CAP().GetByDoiTuong(id, BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri());

                        if (lstTruyCap != null && lstTruyCap.Count > 0)
                        {
                            kq = new DS_HT_TRUY_CAP().XoaTheoList(lstTruyCap);
                        }
                    }

                    // Xóa thông tin nhóm người dùng
                    if (kq == true)
                    {
                        kq = dsNhNsd.Xoa(htNhNsd);
                    }

                    if (kq == true)
                    {
                        trans.Complete();

                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;

                        bsRetDetail.Object = htNhNsd.MA_NHNSD + " (" + htNhNsd.TEN_NHNSD + ")";
                        bsRetDetail.Result = ApplicationConstant.OperationStatus.Successful.layNgonNgu();
                        bsRetDetail.Detail = responseMessage.layGiaTri();

                        return ApplicationConstant.ResponseStatus.THANH_CONG;
                    }
                    else
                    {
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                        bsRetDetail.Object = htNhNsd.MA_NHNSD + " (" + htNhNsd.TEN_NHNSD + ")";
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

        public ApplicationConstant.ResponseStatus XoaListNHNSDTheoId(List<int> listID,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref List<BS_ResponseDetail> listResponseDetail)
        {
            bool tempVal = true;

            foreach (int inf in listID)
            {
                BS_ResponseDetail bsRetDetail = new BS_ResponseDetail();
                bsRetDetail.Id = inf;
                bsRetDetail.Operation = DatabaseConstant.Action.XOA.layNgonNgu();

                ApplicationConstant.ResponseStatus ret = XoaNHNSD(inf, ref responseMessage, ref bsRetDetail);
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    listResponseDetail.Add(bsRetDetail);
                }
                else
                {
                    listResponseDetail.Add(bsRetDetail);
                    tempVal = false;
                }
            }

            if (tempVal)
            {
                //responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            else
            {
                //responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }        

        public List<HT_TSO_LOAI> layLoaiThamSo()
        {
            return new DS_QuanTriHeThong().layLoaiThamSo();
        }

        public List<HT_TSO> layThamSo()
        {
            return new DS_QuanTriHeThong().layThamSo();
        }

        public ApplicationConstant.ResponseStatus layThamSoHeThong(string maDonVi, string loaiThamSo, ref List<HT_TSO> lstHtTso)
        {
            if (loaiThamSo != null)
            {
                // Nếu tham số là TW, BC
                if (loaiThamSo.Equals(BusinessConstant.LoaiThamSo.TW.layGiaTri()) ||
                    loaiThamSo.Equals(BusinessConstant.LoaiThamSo.BC.layGiaTri()))
                {
                    lstHtTso = layThamSoTheoLoaiThamSo(loaiThamSo);
                    return ApplicationConstant.ResponseStatus.THANH_CONG;
                }
                // Nếu tham số là đơn vị
                else if (loaiThamSo.Equals(BusinessConstant.LoaiThamSo.DV.layGiaTri()))
                {
                    lstHtTso = layThamSoDonVi(maDonVi);
                    return ApplicationConstant.ResponseStatus.THANH_CONG;
                }
                else
                {
                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }
            }
            else
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }

        public List<HT_TSO> layThamSoDonVi(string maDonVi)
        {
            return new DS_HT_TSO().layThamSoDonVi(maDonVi);
        }

        public List<HT_TSO> layThamSoDonViTheoLoaiThamSo(string maDonVi, string loaiThamSo)
        {
            return new DS_HT_TSO().layThamSoDonViTheoLoaiThamSo(maDonVi, loaiThamSo);
        }

        public List<HT_TSO> layThamSoTheoLoaiThamSo(string loaiThamSo)
        {
            return new DS_HT_TSO().layThamSoTheoLoaiThamSo(loaiThamSo);
        }

        public ApplicationConstant.ResponseStatus capNhatThamSo(DatabaseConstant.Action action,
                        HT_TSO obj, List<int> listID, ref bool bKetQua, ref int iKetQua,
                        ref ApplicationConstant.NghiepVuResponseMessage responseMessage)
        {
            if (action == DatabaseConstant.Action.LUU || action == DatabaseConstant.Action.LUU_TAM)
            {
                if (obj.ID != null && obj.ID > 0)
                    return SuaThamSo(obj);
                else
                    return ThemThamSo(obj);
            }
            else if (action == DatabaseConstant.Action.XOA)
                return XoaListTheoIDThamSo(listID);
            else if (action == DatabaseConstant.Action.DUYET)
                return DuyetListTheoIDThamSo(listID);
            else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                return TuChoiDuyetListTheoIDThamSo(listID);
            else if (action == DatabaseConstant.Action.THOAI_DUYET)
                return ThoaiDuyetListTheoIDThamSo(listID);
            else
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
        }

        public ApplicationConstant.ResponseStatus capNhatGiaTriThamSo(ref HT_TSO obj,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage,
            ref BS_ResponseDetail bsRetDetail)
        {
            // Start - Lay gia tri cu cua tham so
            string loaiThamSo = obj.MA_TSO_LOAI;
            string giaTriCu = "";
            string giaTriMoi = obj.GIA_TRI;
            try
            {
                if (loaiThamSo.Equals(BusinessConstant.LoaiThamSo.TW.layGiaTri()) ||
                        loaiThamSo.Equals(BusinessConstant.LoaiThamSo.BC.layGiaTri()))
                {
                    HT_TSO htTsoCu = new DS_HT_TSO().GetByMa(obj.MA_TSO);
                    if (htTsoCu != null)
                        giaTriCu = htTsoCu.GIA_TRI;
                    else
                        giaTriCu = "";
                }
                else if (loaiThamSo.Equals(BusinessConstant.LoaiThamSo.DV.layGiaTri()))
                {
                    HT_TSO htTsoCu = new DS_HT_TSO().layThamSoDonVi(BusinessConstant.LoaiThamSo.DV, BusinessConstant.layMaThamSo(obj.MA_TSO), obj.MA_DVI_QLY);
                    if (htTsoCu != null)
                        giaTriCu = htTsoCu.GIA_TRI;
                    else
                        giaTriCu = "";
                }
                else
                {
                    giaTriCu = "";
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            // End - Lay gia tri cu cua tham so

            HT_TSO htTso = new HT_TSO();
            htTso = new DS_HT_TSO().SuaGiaTriThamSo(obj);

            if (htTso != null)
            {
                /* 
                 * Start - Cap nhat HT_TSO_LSU
                 */
                if (!giaTriCu.Equals(giaTriMoi))
                {
                    Entities entities = ContextFactory.GetInstance();
                    try
                    {
                        // Lay ngay lich su cua tham so
                        // Neu tham so TW, BC => lay ngay he thong (machine date), neu tham so DV => lay ngay lam viec don vi (working date)

                        string ngayLichSu = "";

                        if (loaiThamSo.Equals(BusinessConstant.LoaiThamSo.TW.layGiaTri()) ||
                            loaiThamSo.Equals(BusinessConstant.LoaiThamSo.BC.layGiaTri()))
                        {
                            ngayLichSu = LDateTime.GetCurrentDate("yyyyMMdd");
                        }
                        else if (loaiThamSo.Equals(BusinessConstant.LoaiThamSo.DV.layGiaTri()))
                        {
                            string maDonVi = htTso.MA_DVI_QLY;
                            HT_NGAY_LVIEC htNgayLamViec = new BS_CoHeThong().getNgayLamViecTheoDonVi(maDonVi);
                            if (htNgayLamViec != null)
                                ngayLichSu = htNgayLamViec.NGAY_LVIEC;
                            else
                                ngayLichSu = LDateTime.GetCurrentDate("yyyyMMdd");
                        }
                        else
                        {
                            ngayLichSu = LDateTime.GetCurrentDate("yyyyMMdd");
                        }

                        // Cap nhat lich su tham so
                        string nativeQuery = "";
                        nativeQuery =
                                   " INSERT INTO HT_TSO_LSU " +
                                   " (NGAY_LSU " +
                                   " , MA_TSO " +
                                   " , TEN_TSO " +
                                   " , MA_TSO_LOAI " +
                                   " , MA_PHAN_HE " +
                                   " , PHAN_LOAI " +
                                   " , MO_TA " +
                                   " , GIA_TRI " +
                                   " , KIEU_DU_LIEU " +
                                   " , HTHI_DIEU_KHIEN " +
                                   " , HTHI_DVTINH " +
                                   " , HTHI_GTRI_MDINH " +
                                   " , HTHI_SDUNG_TVAN " +
                                   " , HTHI_SQL " +
                                   " , HTHI_POPUP " +
                                   " , NGUON_DL " +
                                   " , PVI_AHUONG " +
                                   " , NGUON_TAO_DL " +
                                   " , TTHAI_BGHI " +
                                   " , TTHAI_NVU " +
                                   " , MA_DVI_QLY " +
                                   " , MA_DVI_TAO " +
                                   " , NGAY_NHAP " +
                                   " , NGUOI_NHAP " +
                                   " , NGAY_CNHAT " +
                                   " , NGUOI_CNHAT " +
                                   " , TTHAI_LY_DO) " +
                                   " VALUES " +
                                   "('" + ngayLichSu + "'" +
                                   " ,'" + htTso.MA_TSO + "'" +
                                   " ,'" + htTso.TEN_TSO + "'" +
                                   " ,'" + htTso.MA_TSO_LOAI + "'" +
                                   " ,'" + htTso.MA_PHAN_HE + "'" +
                                   " ,'" + htTso.PHAN_LOAI + "'" +
                                   " ,'" + htTso.MO_TA + "'" +
                                   " ,'" + giaTriMoi + "'" + // Gia tri moi cua tham so
                                   " ,'" + htTso.KIEU_DU_LIEU + "'" +
                                   " ,'" + htTso.HTHI_DIEU_KHIEN + "'" +
                                   " ,'" + htTso.HTHI_DVTINH + "'" +
                                   " ,'" + htTso.HTHI_GTRI_MDINH + "'" +
                                   " ,'" + htTso.HTHI_SDUNG_TVAN + "'" +
                                   " ,'" + htTso.HTHI_SQL + "'" +
                                   " ,'" + htTso.HTHI_POPUP + "'" +
                                   " ,'" + htTso.NGUON_DL + "'" +
                                   " ,'" + htTso.PVI_AHUONG + "'" +
                                   " ,'" + htTso.NGUON_TAO_DL + "'" +
                                   " ,'" + htTso.TTHAI_BGHI + "'" +
                                   " ,'" + htTso.TTHAI_NVU + "'" +
                                   " ,'" + htTso.MA_DVI_QLY + "'" +
                                   " ,'" + htTso.MA_DVI_TAO + "'" +
                                   " ,'" + htTso.NGAY_NHAP + "'" +
                                   " ,'" + htTso.NGUOI_NHAP + "'" +
                                   " ,'" + htTso.NGAY_CNHAT + "'" +
                                   " ,'" + htTso.NGUOI_CNHAT + "'" +
                                   " ,'" + htTso.TTHAI_LY_DO + "')";


                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, nativeQuery.ToString());
                        var queryResult = entities.ExecuteStoreCommand(nativeQuery);

                    }
                    catch (System.Exception ex)
                    {
                        LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                    }
                    finally
                    {
                        entities = null;
                    }
                }
                /* 
                 * End - Cap nhat HT_TSO_LSU
                 */

                obj = htTso;
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            else
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }

        public ApplicationConstant.ResponseStatus ThemThamSo(HT_TSO obj)
        {
            try
            {
                DS_HT_TSO ds = new DS_HT_TSO();
                ds.Them(obj);
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            catch (Exception ex)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }

        public ApplicationConstant.ResponseStatus SuaThamSo(HT_TSO obj)
        {
            try
            {
                DS_HT_TSO ds = new DS_HT_TSO();
                ds.Sua(obj);
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            catch (Exception ex)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }

        public ApplicationConstant.ResponseStatus XoaListTheoIDThamSo(List<int> listID)
        {
            DS_HT_TSO ds = new DS_HT_TSO();
            if (ds.XoaListTheoID(listID))
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            else
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
        }

        public ApplicationConstant.ResponseStatus DuyetListTheoIDThamSo(List<int> listID)
        {
            DS_HT_TSO ds = new DS_HT_TSO();
            if (ds.DuyetListTheoID(listID))
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            else
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
        }

        public ApplicationConstant.ResponseStatus TuChoiDuyetListTheoIDThamSo(List<int> listID)
        {
            DS_HT_TSO ds = new DS_HT_TSO();
            if (ds.TuChoiDuyetListTheoID(listID))
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            else
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
        }

        public ApplicationConstant.ResponseStatus ThoaiDuyetListTheoIDThamSo(List<int> listID)
        {
            DS_HT_TSO ds = new DS_HT_TSO();
            if (ds.ThoaiDuyetListTheoID(listID))
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            else
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
        }

        public ApplicationConstant.ResponseStatus capNhatLoaiThamSo(DatabaseConstant.Action action,
                        HT_TSO_LOAI obj, List<int> listID, ref bool bKetQua, ref int iKetQua,
                        ref ApplicationConstant.NghiepVuResponseMessage responseMessage)
        {
            if (action == DatabaseConstant.Action.LUU || action == DatabaseConstant.Action.LUU_TAM)
            {
                if (obj.ID != null && obj.ID > 0)
                    return SuaLoaiThamSo(obj);
                else
                    return ThemLoaiThamSo(obj);
            }
            else if (action == DatabaseConstant.Action.XOA)
                return XoaListTheoIDLoaiThamSo(listID);
            else
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
        }

        public ApplicationConstant.ResponseStatus ThemLoaiThamSo(HT_TSO_LOAI obj)
        {
            try
            {
                DS_HT_TSO_LOAI ds = new DS_HT_TSO_LOAI();
                ds.Them(obj);
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            catch (Exception ex)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }

        public ApplicationConstant.ResponseStatus SuaLoaiThamSo(HT_TSO_LOAI obj)
        {
            try
            {
                DS_HT_TSO_LOAI ds = new DS_HT_TSO_LOAI();
                ds.Sua(obj);
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            catch (Exception ex)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
        }

        public ApplicationConstant.ResponseStatus XoaListTheoIDLoaiThamSo(List<int> listID)
        {
            DS_HT_TSO_LOAI ds = new DS_HT_TSO_LOAI();
            if (ds.XoaListTheoID(listID))
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            else
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
        }

        public ApplicationConstant.ResponseStatus DoiMatKhauNguoiDung(string userName,
            string oldPassword,
            string newPassword,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            // Lấy thông tin người dùng
            HT_NSD htNsd = new DS_HT_NSD().GetByMa(userName);

            if (htNsd == null)
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_KhongTonTai;
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }

            // Kiểm tra mật khẩu cũ có chính xác không
            // (Đã mã hóa từ client trước khi truyền thông)
            if (!oldPassword.Equals(htNsd.MAT_KHAU))
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_MatKhauCuKhongChinhXac;
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                htNsd.MAT_KHAU = newPassword;
                htNsd.TDOI_MKHAU = BusinessConstant.YeuCauDoiMatKhau.DA_THAY_DOI.layGiaTri();

                // Lấy ngày làm việc hiện tại của chi nhánh, người dùng thuộc về
                string ngayLamViec = "";
                new BS_CoNgayLamViec().LayNgayLamViec(htNsd.MA_DVI_QLY, out ngayLamViec);
                htNsd.NGAY_DOI_MKHAU = ngayLamViec;

                //htNsd.NGAY_DOI_MKHAU = (DateTime.Today).ToString("yyyyMMdd");
                htNsd = new DS_HT_NSD().Sua(htNsd);

                if (htNsd != null)
                {
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                    return ApplicationConstant.ResponseStatus.THANH_CONG;
                }
                else
                {
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }
            }
        }


        public ApplicationConstant.ResponseStatus ThietLapMatKhauNguoiDung(string userName,
            string password,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            // Lấy thông tin người dùng
            HT_NSD htNsd = new DS_HT_NSD().GetByMa(userName);

            if (htNsd == null)
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_NguoiDung_KhongTonTai;
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }

            else
            {
                htNsd.MAT_KHAU = password;
                htNsd.NGAY_DOI_MKHAU = (DateTime.Today).ToString("yyyyMMdd");
                htNsd = new DS_HT_NSD().Sua(htNsd);

                if (htNsd != null)
                {
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                    return ApplicationConstant.ResponseStatus.THANH_CONG;
                }
                else
                {
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }
            }
        }
    }
}
