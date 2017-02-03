using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    public class DS_HT_TRUY_CAP
    {
        private string ENTITY_SET_NAME = "HT_TRUY_CAP";

        public HT_TRUY_CAP Them(HT_TRUY_CAP obj)
        {
            HT_TRUY_CAP kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_TRUY_CAP.AddObject(obj);
                entities.SaveChanges();
                kq = obj;
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
            return kq;
        }
        
        public HT_TRUY_CAP Sua(HT_TRUY_CAP obj)
        {
            HT_TRUY_CAP kq = null;
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
                kq = obj;
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
            return kq;
        }

        public bool Xoa(HT_TRUY_CAP obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_TRUY_CAP.Attach(obj);
                entities.HT_TRUY_CAP.DeleteObject(obj);
                entities.SaveChanges();
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool XoaTheoList(List<HT_TRUY_CAP> lst)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                foreach (HT_TRUY_CAP obj in lst)
                {
                    entities.HT_TRUY_CAP.Attach(obj);
                    entities.DeleteObject(obj);
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
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

        public List<HT_TRUY_CAP> GetByDoiTuong(int IDDoiTuong, string loaiDoiTuong)
        {
            List<HT_TRUY_CAP> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                var query = from e in entities.HT_TRUY_CAP
                            where e.ID_DTUONG == IDDoiTuong && e.LOAI_DTUONG.Equals(loaiDoiTuong)
                            select e;
                kq = query.ToList();             
            }
            catch (Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;

        }

        public List<HT_TRUY_CAP> GetByListIdDoiTuong(List<int> lstIDDoiTuong, string loaiDoiTuong)
        {
            List<HT_TRUY_CAP> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_TRUY_CAP.Where(e => lstIDDoiTuong.Contains(e.ID_DTUONG) && e.LOAI_DTUONG.Equals(loaiDoiTuong)).ToList();
            }
            catch (Exception ex)
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

        public List<HT_TRUY_CAP> GetByDoiTuong(int IDDoiTuong, string loaiDoiTuong, string kichHoat)
        {
            List<HT_TRUY_CAP> kq = null;

            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    var query = from e in entities.HT_TRUY_CAP
                                where e.ID_DTUONG == IDDoiTuong && e.LOAI_DTUONG.Equals(loaiDoiTuong) && e.KICH_HOAT.Equals(kichHoat)
                                select e;
                    kq = query.ToList();
                }
            }
            catch (Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;

        }
    }
}
