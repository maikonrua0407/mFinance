using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using Utilities.Common;
using System.Data;
using System.Transactions;

namespace DataServices.QuanTriHeThong
{
    public class DS_HT_NSD_PVI
    {
        private string ENTITY_SET_NAME = "HT_NSD_PVI";

        public HT_NSD_PVI Them(HT_NSD_PVI obj)
        {
            HT_NSD_PVI kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NSD_PVI.AddObject(obj);
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

        public HT_NSD_PVI Sua(HT_NSD_PVI obj)
        {
            HT_NSD_PVI kq = null;
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

        public bool Xoa(HT_NSD_PVI obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NSD_PVI.DeleteObject(obj);
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

        public bool XoaTheoID_NSD(int idNSD)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    List<HT_NSD_PVI> lst = new List<HT_NSD_PVI>();
                    lst = entities.HT_NSD_PVI.Where(e => e.ID_NSD == idNSD).ToList();
                    foreach (HT_NSD_PVI obj in lst)
                    {
                        entities.HT_NSD_PVI.Attach(obj);
                        entities.HT_NSD_PVI.DeleteObject(obj);
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

        public bool XoaTheoID_NSD_MaLoaiPhamVi(int idNSD, string maLoaiPhamVi)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    List<HT_NSD_PVI> lst = new List<HT_NSD_PVI>();
                    lst = entities.HT_NSD_PVI.Where(e => e.ID_NSD == idNSD && e.MA_PVI_LOAI.Equals(maLoaiPhamVi)).ToList();
                    foreach (HT_NSD_PVI obj in lst)
                    {
                        entities.HT_NSD_PVI.Attach(obj);
                        entities.HT_NSD_PVI.DeleteObject(obj);
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

        public List<HT_NSD_PVI> GetByIdNSDAndMaLoaiPhamVi(int id_NSD, string maLoaiPhamVi)
        {
            List<HT_NSD_PVI> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_NSD_PVI.Where(e => e.ID_NSD == id_NSD && e.MA_PVI_LOAI.Equals(maLoaiPhamVi)).ToList();
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
