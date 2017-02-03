using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Reflection;
using DataModel.EntityFramework;
using Utilities.Common;
using NG.ADO;
using System.Collections;
using System.Transactions;

namespace DataServices.QuanTriHeThong
{
    /// <summary>
    /// Lớp chứa các phương thức liên quan đến khai báo, lấy và kiểm tra quyền truy cập
    /// </summary>
    public class DS_PhanQuyen
    {
        /// <summary>
        /// Lấy danh sách Tài nguyên khai thác của người dùng trong hệ thống
        /// </summary>
        /// <param name="maDangNhap">Mã hay Tài khoản đăng nhập</param>
        /// <param name="maDonVi">Mã đơn vị</param>
        /// <returns>Danh sách dạng DataTable Tài nguyên khai thác của người dùng trong hệ thống</returns>
        public DataSet LayTNuyenKThacTheoUser(string maDangNhap, string maDonVi)
        {
            string sql = "";
            try
            {
                LDataAccessLayer DAL = new LDataAccessLayer();
                sql = @"EXEC	[dbo].[SP_SELECT_LayDSachCNangcntnng] '" + maDangNhap + "','" + maDonVi + "'";
                DataSet ds = DAL.DataSetExecuteADO(sql);
                // Ghi Log câu truy vấn
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        /// <summary>
        /// Lấy danh sách cây menu của webclient theo người dùng
        /// </summary>
        /// <param name="maDangNhap">Mã hay Tài khoản đăng nhập</param>
        /// <param name="maDonVi">Mã đơn vị</param>
        /// <returns>Danh sách dạng DataTable danh sách chức năng của người dùng trong hệ thống</returns>
        public DataSet LayMenuTheoUser(string maDangNhap, string maDonVi)
        {
            string sql = "";
            try
            {
                // Kiểm tra timing
                LDataAccessLayer DAL = new LDataAccessLayer();
                sql = @"wsp_User_LayMenuTheoUser '" + maDangNhap + "','" + maDonVi + "'";
                DataSet ds = DAL.ExecuteQuery(sql);
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách Chức năng của người dùng trong hệ thống
        /// </summary>
        /// <param name="maDangNhap">Mã hay Tài khoản đăng nhập</param>
        /// <param name="maDonVi">Mã đơn vị</param>
        /// <returns>Danh sách dạng DataTable danh sách chức năng của người dùng trong hệ thống</returns>
        public DataSet LayDanhSachChucNangTheoUser(string maDangNhap, string maDonVi)
        {
            string sql = "";
            try
            {
                // Kiểm tra timing
                LDataAccessLayer DAL = new LDataAccessLayer();
                sql = @"EXEC	[dbo].[SP_SELECT_LayDSachCNangTNang] '" + maDangNhap + "','" + maDonVi + "'";
                DataSet ds = DAL.DataSetExecuteADO(sql);
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public DataSet LayDanhSachChucNangWebTheoUser(string maDangNhap, string maDonVi)
        {
            string sql = "";
            try
            {
                // Kiểm tra timing
                LDataAccessLayer DAL = new LDataAccessLayer();
                sql = @"EXEC	[dbo].[SP_SELECT_LayDSachCNangTNang_Web] '" + maDangNhap + "','" + maDonVi + "'";
                DataSet ds = DAL.DataSetExecuteADO(sql);
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public bool check()
        {
            return true;
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public class DanhSachChucNang
        {
            public HT_CNANG ChucNang;
            public List<HT_TNANG> ListTinhNang;
        }

        public List<HT_CNANG_TNANG> layCNangTNang()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG_TNANG.ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdChucNang(List<int> lstIdChucNang)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG_TNANG.Where(e => lstIdChucNang.Contains(e.ID_CNANG)).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdMenu(List<int> lstIdMenu)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                var lstIdChucNang = entities.HT_MENU.Where(e => lstIdMenu.Contains(e.ID)).Select(e => e.ID_CNANG).ToList();
                if (lstIdChucNang.IsNullOrEmpty() || lstIdChucNang.Count == 0) return null;
                return entities.HT_CNANG_TNANG.Where(e => lstIdChucNang.Contains(e.ID_CNANG)).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdCNangTNang(List<int> lstIdCNangTNang)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG_TNANG.Where(e => lstIdCNangTNang.Contains(e.ID)).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG> layCNang()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG.ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG> layCNangTheoListIdChucNang(List<int> lstIdChucNang)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG.Where(e => lstIdChucNang.Contains(e.ID)).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG> layCNangTheoPhanHe(string maPhanHe)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();

                List<HT_CNANG> lstCNangTheoPhanHe = new List<HT_CNANG>();
                List<int> lstIdCNangCha = new List<int>();
                List<int> lstIdCNangKhongSuDung = new List<int>();

                lstCNangTheoPhanHe = entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe)).ToList();
                lstIdCNangCha = lstCNangTheoPhanHe.Where(e => e.ID_CNANG_CHA != null).Select(m => m.ID_CNANG_CHA.Value).Distinct().ToList();
                //lstIdCNangCha = entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe) && e.ID_CNANG_CHA != null).Select(m => m.ID_CNANG_CHA.Value).Distinct().ToList();
                lstIdCNangKhongSuDung = lstCNangTheoPhanHe.Where(e => e.PHAN_LOAI.Equals("00000")).Select(m => m.ID).Distinct().ToList();
                //lstIdCNangKhongSuDung = entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe) && e.PHAN_LOAI.Equals("00000")).Select(m => m.ID).Distinct().ToList();

                return lstCNangTheoPhanHe.Where(e => !lstIdCNangCha.Contains(e.ID) && !lstIdCNangKhongSuDung.Contains(e.ID)).OrderBy(item => item.SO_TTU).ToList();
                /*
                if (maPhanHe != null && !maPhanHe.Equals(""))
                    return entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe) && !lstIdCNangCha.Contains(e.ID) && !lstIdCNangKhongSuDung.Contains(e.ID)).ToList();
                else
                    return entities.HT_CNANG.ToList();
                */
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG> layCNangThietLapAPTheoPhanHe(string maPhanHe)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();

                List<HT_CNANG> lstCNangTheoPhanHe = new List<HT_CNANG>();
                List<int> lstIdCNangCha = new List<int>();
                List<int> lstIdCNangKhongSuDung = new List<int>();

                lstCNangTheoPhanHe = entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe)).ToList();
                lstIdCNangCha = lstCNangTheoPhanHe.Where(e => e.ID_CNANG_CHA != null).Select(m => m.ID_CNANG_CHA.Value).Distinct().ToList();
                //lstIdCNangCha = entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe) && e.ID_CNANG_CHA != null).Select(m => m.ID_CNANG_CHA.Value).Distinct().ToList();
                lstIdCNangKhongSuDung = lstCNangTheoPhanHe.Where(e => e.PHAN_LOAI.Equals("00000")).Select(m => m.ID).Distinct().ToList();
                //lstIdCNangKhongSuDung = entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe) && e.PHAN_LOAI.Equals("00000")).Select(m => m.ID).Distinct().ToList();

                return lstCNangTheoPhanHe.Where(e => !lstIdCNangCha.Contains(e.ID) && !lstIdCNangKhongSuDung.Contains(e.ID) && e.CAP_PHE_DUYET > 1).ToList();
                /*
                if (maPhanHe != null && !maPhanHe.Equals(""))
                    return entities.HT_CNANG.Where(e => e.MODULE.Equals(maPhanHe) && !lstIdCNangCha.Contains(e.ID) && !lstIdCNangKhongSuDung.Contains(e.ID)).ToList();
                else
                    return entities.HT_CNANG.ToList();
                */
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyen()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG_PQUYEN.ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyenTheoDoiTuong(string maDoiTuong, string loaiDoiTuong)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG_PQUYEN.Where(e => e.MA_DTUONG.Equals(maDoiTuong) && e.MA_DTUONG_LOAI.Equals(loaiDoiTuong)).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyenTheoDoiTuongChucNang(string maDoiTuong, string loaiDoiTuong, List<int> lstIdChucNang)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();

                // Lấy danh sách CNangTNang theo lstIdChucNang
                List<HT_CNANG_TNANG> lstCNangTNang = layCNangTNangTheoListIdChucNang(lstIdChucNang);
                List<int> lstIdCNangTNang = lstCNangTNang.Select(e => e.ID).ToList();

                return entities.HT_CNANG_PQUYEN.Where(e => e.MA_DTUONG.Equals(maDoiTuong) && e.MA_DTUONG_LOAI.Equals(loaiDoiTuong) && lstIdCNangTNang.Contains(e.ID_CNANG_TNANG.Value)).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TNANG> layTNang()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_TNANG.ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TNANG> layTNangTheoListIdTinhNang(List<int> lstIdTinhNang)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_TNANG.Where(e => lstIdTinhNang.Contains(e.ID)).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TNANG> layTNangDuocPhanQuyen()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_TNANG.Where(e => e.PHAN_QUYEN.Equals("CO")).ToList();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TNGUYEN> layTNguyen(string maTNguyen, string loaiTNguyen)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                List<HT_TNGUYEN> lst = entities.HT_TNGUYEN.ToList();
                if (!string.IsNullOrEmpty(maTNguyen) && maTNguyen != "")
                    lst = lst.Where(e => e.MA_TNGUYEN.Equals(maTNguyen) || e.MA_TNGUYEN_CHA.Equals(maTNguyen)).ToList();
                if (!string.IsNullOrEmpty(loaiTNguyen) && loaiTNguyen != "")
                    lst = lst.Where(e => e.MA_TNGUYEN_LOAI.Equals(loaiTNguyen)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TNGUYEN_KTHAC> layDSTNguyenKThac(string maNSD, string loaiNSD, string loaiTNguyen = null)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                List<HT_TNGUYEN_KTHAC> lst = entities.HT_TNGUYEN_KTHAC.Where(e => e.MA_DTUONG.Equals(maNSD) && e.MA_DTUONG_LOAI.Equals(loaiNSD)).ToList();
                if (!string.IsNullOrEmpty(loaiTNguyen) && loaiTNguyen != "")
                    lst = lst.Where(e => e.MA_TNGUYEN_LOAI.Equals(loaiTNguyen)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public bool luuPhanQuyen(string maDoiTuong, string loaiDoiTuong, ArrayList lstPhanQuyen, string nguoiCapNhat)
        {
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    Entities entities = ContextFactory.GetInstance();
                    List<string> lstMaCNang = new List<string>();
                    string taiNguyenMenu = BusinessConstant.LoaiTaiNguyen.MENU.layGiaTri();
                    foreach (List<string> lst in lstPhanQuyen)
                    {
                        // Xóa hết phân quyền cũ
                        foreach (string tn in lst)
                        {
                            HT_TNGUYEN_KTHAC tnkt = entities.HT_TNGUYEN_KTHAC.FirstOrDefault(e => e.MA_DTUONG.Equals(maDoiTuong) && e.MA_DTUONG_LOAI.Equals(loaiDoiTuong) && e.MA_TNGUYEN.Equals(tn) && e.MA_TNGUYEN_LOAI.Equals(taiNguyenMenu));
                            if (tnkt != null)
                            {
                                entities.HT_TNGUYEN_KTHAC.DeleteObject(tnkt);
                                entities.SaveChanges();
                            }
                            if (lst.IndexOf(tn) > 0)
                            {
                                if (!lstMaCNang.Contains(tn))
                                    lstMaCNang.Add(tn);
                                // ID của tính năng được gán vào tham số Capacity khi khởi tạo list phía client
                                int idTinhNang = Convert.ToInt32(lst.First());
                                HT_CNANG_TNANG cntn = entities.HT_CNANG_TNANG.FirstOrDefault(e => e.MA_CNANG.Equals(tn) && e.ID_TNANG == idTinhNang);
                                // ID của tính năng được lưu trong trường GTRI_TNGUYEN
                                HT_CNANG_PQUYEN cnpq = entities.HT_CNANG_PQUYEN.FirstOrDefault(e => e.MA_DTUONG.Equals(maDoiTuong) && e.MA_DTUONG_LOAI.Equals(loaiDoiTuong) && e.ID_CNANG_TNANG == cntn.ID);
                                if (cnpq != null)
                                {
                                    entities.HT_CNANG_PQUYEN.DeleteObject(cnpq);
                                    entities.SaveChanges();
                                }
                            }
                        }
                        foreach (string tn in lst)
                        {
                            if (lst.IndexOf(tn) > 0)
                            {
                                HT_CNANG_PQUYEN obj = new HT_CNANG_PQUYEN();
                                // ID của tính năng được gán vào tham số Capacity khi khởi tạo list phía client
                                int idTinhNang = Convert.ToInt32(lst.First());
                                HT_CNANG_TNANG cntn = entities.HT_CNANG_TNANG.FirstOrDefault(e => e.MA_CNANG.Equals(tn) && e.ID_TNANG == idTinhNang);

                                obj.ID_CNANG_TNANG = cntn.ID;
                                if (loaiDoiTuong.Equals(BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri()))
                                    obj.ID_DTUONG = entities.HT_NSD.FirstOrDefault(e => e.MA_NSD.Equals(maDoiTuong)).ID;

                                if (loaiDoiTuong.Equals(BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri()))
                                    obj.ID_DTUONG = entities.HT_NHNSD.FirstOrDefault(e => e.MA_NHNSD.Equals(maDoiTuong)).ID;

                                obj.MA_DTUONG = maDoiTuong;
                                obj.MA_DTUONG_LOAI = loaiDoiTuong;
                                obj.MA_DVI_QLY = entities.HT_NSD.FirstOrDefault(e => e.MA_DANG_NHAP.ToLower().Equals(nguoiCapNhat.ToLower())).MA_DVI_QLY;
                                obj.MA_DVI_TAO = obj.MA_DVI_QLY;
                                obj.NGAY_CNHAT = DateTime.Today.ToString("yyyyMMdd");
                                obj.NGAY_NHAP = DateTime.Today.ToString("yyyyMMdd");
                                obj.NGUOI_CNHAT = nguoiCapNhat;
                                obj.NGUOI_NHAP = nguoiCapNhat;
                                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                                entities.HT_CNANG_PQUYEN.AddObject(obj);
                                entities.SaveChanges();
                            }
                        }
                    }

                    // Tạo quyền mới
                    foreach (string maCNang in lstMaCNang)
                    {
                        HT_TNGUYEN_KTHAC obj = new HT_TNGUYEN_KTHAC();

                        HT_CNANG cn = entities.HT_CNANG.FirstOrDefault(e => e.MA_CNANG.Equals(maCNang));

                        obj.GTRI_TNGUYEN = cn.ID.ToString();
                        obj.ID_TNGUYEN = cn.ID;
                        obj.MA_TNGUYEN = cn.MA_CNANG;

                        obj.ID_TNGUYEN_CHA = cn.ID_CNANG_CHA;
                        if (cn.ID_CNANG_CHA != null)
                            obj.MA_TNGUYEN_CHA = entities.HT_CNANG.FirstOrDefault(e => e.ID == cn.ID_CNANG_CHA).MA_CNANG;

                        obj.TEN_TNGUYEN = cn.TEN_CNANG;
                        if (loaiDoiTuong.Equals(BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri()))
                            obj.ID_DTUONG = entities.HT_NSD.FirstOrDefault(e => e.MA_NSD.Equals(maDoiTuong)).ID;

                        if (loaiDoiTuong.Equals(BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri()))
                            obj.ID_DTUONG = entities.HT_NHNSD.FirstOrDefault(e => e.MA_NHNSD.Equals(maDoiTuong)).ID;

                        obj.ID_TNGUYEN_LOAI = entities.HT_TNGUYEN_LOAI.FirstOrDefault(e => e.MA_TNGUYEN_LOAI.Equals(taiNguyenMenu)).ID;
                        obj.MA_DTUONG = maDoiTuong;
                        obj.MA_DTUONG_LOAI = loaiDoiTuong;
                        obj.KIEU_DLIEU = "String";
                        obj.MA_DVI_QLY = entities.DM_DON_VI.First().MA_DVI;
                        obj.MA_DVI_TAO = obj.MA_DVI_QLY;
                        obj.MA_TNGUYEN_LOAI = BusinessConstant.LoaiTaiNguyen.MENU.layGiaTri();
                        obj.NGAY_CNHAT = DateTime.Today.ToString("yyyyMMdd");
                        obj.NGAY_NHAP = DateTime.Today.ToString("yyyyMMdd");
                        obj.NGUOI_CNHAT = nguoiCapNhat;
                        obj.NGUOI_NHAP = nguoiCapNhat;
                        obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        entities.HT_TNGUYEN_KTHAC.AddObject(obj);
                        entities.SaveChanges();

                        if (cn.ID_CNANG_CHA != null)
                            InsertParrentTNKT(cn.ID_CNANG_CHA.Value, maDoiTuong, obj.ID_DTUONG, loaiDoiTuong, nguoiCapNhat);
                    }
                    trans.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public bool luuPhanQuyen(string maDoiTuong, string loaiDoiTuong)
        {
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    Entities entities = ContextFactory.GetInstance();
                }
                return true;

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void InsertParrentTNKT(int idCNang, string maDoiTuong, int idDoiTuong, string loaiDoiTuong, string nguoiCapNhat)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                HT_TNGUYEN_KTHAC obj = new HT_TNGUYEN_KTHAC();
                string taiNguyenMenu = BusinessConstant.LoaiTaiNguyen.MENU.layGiaTri();

                HT_CNANG cn = entities.HT_CNANG.FirstOrDefault(e => e.ID == idCNang);
                obj.GTRI_TNGUYEN = cn.ID.ToString();
                obj.ID_TNGUYEN = cn.ID;
                obj.MA_TNGUYEN = cn.MA_CNANG;

                obj.ID_TNGUYEN_CHA = cn.ID_CNANG_CHA;
                if (cn.ID_CNANG_CHA != null)
                    obj.MA_TNGUYEN_CHA = entities.HT_CNANG.FirstOrDefault(e => e.ID == cn.ID_CNANG_CHA).MA_CNANG;

                obj.TEN_TNGUYEN = cn.TEN_CNANG;
                obj.ID_DTUONG = idDoiTuong;

                obj.ID_TNGUYEN_LOAI = entities.HT_TNGUYEN_LOAI.FirstOrDefault(e => e.MA_TNGUYEN_LOAI.Equals(taiNguyenMenu)).ID;
                obj.MA_DTUONG = maDoiTuong;
                obj.MA_DTUONG_LOAI = loaiDoiTuong;
                obj.KIEU_DLIEU = "String";
                obj.MA_DVI_QLY = entities.DM_DON_VI.First().MA_DVI;
                obj.MA_DVI_TAO = obj.MA_DVI_QLY;
                obj.MA_TNGUYEN_LOAI = BusinessConstant.LoaiTaiNguyen.MENU.layGiaTri();
                obj.NGAY_CNHAT = DateTime.Today.ToString("yyyyMMdd");
                obj.NGAY_NHAP = DateTime.Today.ToString("yyyyMMdd");
                obj.NGUOI_CNHAT = nguoiCapNhat;
                obj.NGUOI_NHAP = nguoiCapNhat;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                entities.HT_TNGUYEN_KTHAC.AddObject(obj);
                entities.SaveChanges();
                if (cn.ID_CNANG_CHA != null)
                    InsertParrentTNKT(cn.ID_CNANG_CHA.Value, maDoiTuong, idDoiTuong, loaiDoiTuong, nguoiCapNhat);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
