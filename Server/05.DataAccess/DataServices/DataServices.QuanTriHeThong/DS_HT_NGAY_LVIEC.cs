using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    /// <summary>
    /// Ngày làm việc trước, hiện tại, tiếp theo
    /// </summary>
    public class DS_HT_NGAY_LVIEC
    {
        /// <summary>
        /// Lấy thông tin ngày làm việc của đơn vị
        /// </summary>
        public HT_NGAY_LVIEC getNgayLamViecTheoDonVi(string maDonVi)
        {
            Entities entities = ContextFactory.GetInstance();
            HT_NGAY_LVIEC htNgayLamViec = null;
            try
            {
                htNgayLamViec = entities.HT_NGAY_LVIEC.FirstOrDefault(e => e.MA_DVI.Equals(maDonVi));
                if (htNgayLamViec != null)
                {
                    return htNgayLamViec;
                }
            }
            catch (System.Exception ex)
            {
                htNgayLamViec = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                entities = null;
            }
            return htNgayLamViec;
        }

        private string ENTITY_SET_NAME = "HT_NGAY_LVIEC";

        public bool Them(HT_NGAY_LVIEC obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NGAY_LVIEC.AddObject(obj);
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

        public bool Sua(HT_NGAY_LVIEC obj)
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

        public bool Xoa(HT_NGAY_LVIEC obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NGAY_LVIEC.Attach(obj);
                entities.HT_NGAY_LVIEC.DeleteObject(obj);
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
                        HT_NGAY_LVIEC obj = new HT_NGAY_LVIEC();
                        obj = entities.HT_NGAY_LVIEC.FirstOrDefault(e => e.ID == id);
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
                        HT_NGAY_LVIEC obj = new HT_NGAY_LVIEC();
                        obj = entities.HT_NGAY_LVIEC.FirstOrDefault(e => e.ID == id);
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
                        HT_NGAY_LVIEC obj = new HT_NGAY_LVIEC();
                        obj = entities.HT_NGAY_LVIEC.FirstOrDefault(e => e.ID == id);
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
                        HT_NGAY_LVIEC obj = new HT_NGAY_LVIEC();
                        obj = entities.HT_NGAY_LVIEC.FirstOrDefault(e => e.ID == id);
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

        public HT_NGAY_LVIEC GetById(int id)
        {
            HT_NGAY_LVIEC kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_NGAY_LVIEC.FirstOrDefault(e => e.ID == id);
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

        public HT_NGAY_LVIEC GetByMaDonVi(string maDonVi)
        {
            HT_NGAY_LVIEC kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_NGAY_LVIEC.FirstOrDefault(e => e.MA_DVI.Equals(maDonVi));
                return kq;
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                entities = null;
            }
        }

        public string getNgayLamViecChoTW()
        {
            string kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                string ngayDG = entities.HT_NGAY_LVIEC.Where(e => e.TTHAI_GDICH.Equals(BusinessConstant.TRANG_THAI_GIAO_DICH.GIAO_DICH)).Min(e => e.NGAY_LVIEC);
                string ngayTruoc = entities.HT_NGAY_LVIEC.Where(e => !e.TTHAI_GDICH.Equals(BusinessConstant.TRANG_THAI_GIAO_DICH.GIAO_DICH)).Min(e => e.NGAY_TRUOC);
                if (ngayTruoc.CompareTo(ngayDG) < 0)
                    kq = ngayTruoc;
                else
                    kq = ngayDG;
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

    }
}
