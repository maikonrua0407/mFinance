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
    public class DS_HT_CNANG_HDSD
    {
        private string ENTITY_SET_NAME = "HT_CNANG_HDSD";

        public bool Them(HT_CNANG_HDSD obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_CNANG_HDSD.AddObject(obj);
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

        public bool Sua(HT_CNANG_HDSD obj)
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

        public bool Xoa(HT_CNANG_HDSD obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.DeleteObject(obj);
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
                        HT_CNANG_HDSD obj = new HT_CNANG_HDSD();
                        obj = entities.HT_CNANG_HDSD.FirstOrDefault(e => e.ID == id);
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

        

        public HT_CNANG_HDSD GetById(int id)
        {
            HT_CNANG_HDSD kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_CNANG_HDSD.FirstOrDefault(e => e.ID == id);
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

        public HT_CNANG_HDSD GetByMa(string ma)
        {
            HT_CNANG_HDSD kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                kq = entities.HT_CNANG_HDSD.FirstOrDefault(e => e.MA_HDSD.Equals(ma));
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
