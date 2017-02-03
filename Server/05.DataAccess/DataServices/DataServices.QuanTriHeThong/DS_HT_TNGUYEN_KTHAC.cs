using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    public class DS_HT_TNGUYEN_KTHAC
    {
        /// <summary>
        /// Lấy danh sách Tài nguyên khai thác theo NSD
        /// </summary>
        /// <param name="id_NSD">ID Người sử dụng</param>
        /// <param name="maDonVi">Mã đơn vị quản lý</param>
        /// <returns>List Tài nguyên khai thác theo NSD</returns>
        public List<HT_TNGUYEN_KTHAC> LayTNguyenKThacNSD(int id_NSD, string maDonVi)
        {
            // Lấy giá trị tham số khai báo trong Constant
            string dtuong_NSD = BusinessConstant.layGiaTri(BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG);
            string dtuong_NHNSD = BusinessConstant.layGiaTri(BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG);
            string trangThaiBanGhi = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            string trangThaiNghiepVu = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.DA_DUYET);
            string[] loaiTaiNguyen = new string[3];
            loaiTaiNguyen[0] = BusinessConstant.layGiaTri(BusinessConstant.LoaiTaiNguyen.MENU);
            loaiTaiNguyen[1] = BusinessConstant.layGiaTri(BusinessConstant.LoaiTaiNguyen.FORM);
            loaiTaiNguyen[2] = BusinessConstant.layGiaTri(BusinessConstant.LoaiTaiNguyen.TINH_NANG);
            
            // Entities entities = new Entities();
            Entities entities = ContextFactory.GetInstance();
            // Lấy danh sách ID Nhóm của NSD
            var ID_NHNSD = entities.HT_NHNSD_NSD.Where(e => e.ID_NSD == id_NSD).Select(e => e.ID_NHNSD);
            // Lấy danh sách Tài nguyên khai thác cho NSD
            //List<HT_TNGUYEN_KTHAC> TNKT = entities.HT_TNGUYEN_KTHAC.Where(e => ((e.ID_DTUONG.Value == id_NSD && e.MA_DTUONG_LOAI.Equals(dtuong_NSD)) || (ID_NHNSD.Contains(e.ID_DTUONG) && e.MA_DTUONG_LOAI.Equals(dtuong_NHNSD))) && e.MA_DVI_QLY.Equals(maDonVi) && e.TTHAI_BGHI.Equals(trangThaiBanGhi) && e.TTHAI_NVU.Equals(trangThaiNghiepVu) && loaiTaiNguyen.Contains(e.MA_TNGUYEN_LOAI)).ToList();
            List<HT_TNGUYEN_KTHAC> TNKT = entities.HT_TNGUYEN_KTHAC.Where(e => ((e.ID_DTUONG == id_NSD && e.MA_DTUONG_LOAI.Equals(dtuong_NSD)) || (ID_NHNSD.Contains(e.ID_DTUONG) && e.MA_DTUONG_LOAI.Equals(dtuong_NHNSD))) && e.MA_DVI_QLY.Equals(maDonVi) && e.TTHAI_BGHI.Equals(trangThaiBanGhi) && e.TTHAI_NVU.Equals(trangThaiNghiepVu) && loaiTaiNguyen.Contains(e.MA_TNGUYEN_LOAI)).ToList();
            return TNKT;
        }

		private string ENTITY_SET_NAME = "HT_TNGUYEN_KTHAC";

        public bool Them(HT_TNGUYEN_KTHAC obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_TNGUYEN_KTHAC.AddObject(obj);
                entities.SaveChanges();
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool Sua(HT_TNGUYEN_KTHAC obj)
        {
            bool kq = true;            
            Entities entities = ContextFactory.GetInstance();
            EntityKey key = null;
            object original = null;
            try
            {
                key = entities.CreateEntityKey(ENTITY_SET_NAME, obj);
                if (entities.TryGetObjectByKey(key, out original))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, obj);
                }
                entities.SaveChanges();
                kq = true;
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool Xoa(HT_TNGUYEN_KTHAC obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_TNGUYEN_KTHAC.DeleteObject(obj);
                entities.SaveChanges();
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool XoaListTheoID(List<int> listID)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (int id in listID)
                    {
                        HT_TNGUYEN_KTHAC obj = new HT_TNGUYEN_KTHAC();
                        obj = entities.HT_TNGUYEN_KTHAC.FirstOrDefault(e => e.ID == id);
                        entities.DeleteObject(obj);
                        entities.SaveChanges();
                    }
                    trans.Complete();
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool DuyetListTheoID(List<int> listID)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (int id in listID)
                    {
                        HT_TNGUYEN_KTHAC obj = new HT_TNGUYEN_KTHAC();
                        obj = entities.HT_TNGUYEN_KTHAC.FirstOrDefault(e => e.ID == id);
                        obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        entities.SaveChanges();
                    }
                    trans.Complete();
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool TuChoiDuyetListTheoID(List<int> listID)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (int id in listID)
                    {
                        HT_TNGUYEN_KTHAC obj = new HT_TNGUYEN_KTHAC();
                        obj = entities.HT_TNGUYEN_KTHAC.FirstOrDefault(e => e.ID == id);
                        obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                        entities.SaveChanges();
                    }
                    trans.Complete();
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool HuyDuyetListTheoID(List<int> listID)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (int id in listID)
                    {
                        HT_TNGUYEN_KTHAC obj = new HT_TNGUYEN_KTHAC();
                        obj = entities.HT_TNGUYEN_KTHAC.FirstOrDefault(e => e.ID == id);
                        obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                        entities.SaveChanges();
                    }
                    trans.Complete();
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public HT_TNGUYEN_KTHAC GetById(int id)
        {
            HT_TNGUYEN_KTHAC kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_TNGUYEN_KTHAC.FirstOrDefault(e => e.ID == id);
                return kq;
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public HT_TNGUYEN_KTHAC GetByMa(string ma)
        {
            HT_TNGUYEN_KTHAC kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                kq = entities.HT_TNGUYEN_KTHAC.FirstOrDefault(e => e.MA_TNGUYEN.Equals(ma));
                return kq;
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return kq;
        }


        public bool XoaTheoDoiTuong(int idDoiTuong, string maDoiTuong)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                List<HT_TNGUYEN_KTHAC> lstTNguyenKThac = entities.HT_TNGUYEN_KTHAC.Where(e => e.ID_DTUONG == idDoiTuong && e.MA_DTUONG.Equals(maDoiTuong)).ToList();

                foreach(HT_TNGUYEN_KTHAC item in lstTNguyenKThac)
                {
                    entities.HT_TNGUYEN_KTHAC.Attach(item);
                    entities.HT_TNGUYEN_KTHAC.DeleteObject(item);
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public bool XoaTheoDoiTuong(int idDoiTuong, string maDoiTuong, string loaiDoiTuong)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                List<HT_TNGUYEN_KTHAC> lstTNguyenKThac = entities.HT_TNGUYEN_KTHAC.Where(e => e.ID_DTUONG == idDoiTuong && e.MA_DTUONG == maDoiTuong && e.MA_DTUONG_LOAI.Equals(loaiDoiTuong)).ToList();

                foreach (HT_TNGUYEN_KTHAC item in lstTNguyenKThac)
                {
                    entities.HT_TNGUYEN_KTHAC.Attach(item);
                    entities.HT_TNGUYEN_KTHAC.DeleteObject(item);
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
