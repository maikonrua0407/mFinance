using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using DataModel.EntityFramework;
using Utilities.Common;
using System.Data.Objects;

namespace DataServices.QuanTriHeThong
{
    public class DS_HT_SO_TT
    {
        private string ENTITY_SET_NAME = "HT_SO_TT";

        public bool Them(HT_SO_TT obj)
        {
            bool kq = true;
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    entities.HT_SO_TT.AddObject(obj);
                    entities.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;
        }

        public bool Sua(HT_SO_TT obj)
        {
            bool kq = true;
            EntityKey key = null;
            object original = null;
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    key = entities.CreateEntityKey(ENTITY_SET_NAME, obj);
                    if (entities.TryGetObjectByKey(key, out original))
                    {
                        entities.ApplyCurrentValues(key.EntitySetName, obj);
                    }
                    entities.SaveChanges();
                    kq = true;
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;
        }

        public bool SuaSoTiepTheo(HT_SO_TT obj)
        {
            bool kq = true;
            
            try
            {
                TransactionOptions transOption = new TransactionOptions();
                //transOption.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
                transOption.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
                transOption.Timeout = TransactionManager.MaximumTimeout;
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transOption))
                {
                    using (Entities entities = ContextFactory.GetInstance())
                    {
                        HT_SO_TT objOriginal = entities.HT_SO_TT.FirstOrDefault(e => e.MA_PHAN_HE.Equals(obj.MA_PHAN_HE) && e.GTRI_TPHAN.Equals(obj.GTRI_TPHAN));
                        objOriginal.SO_TIEP_THEO = obj.SO_TIEP_THEO;
                        entities.SaveChanges();
                        kq = true;
                        transaction.Complete();
                    }
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;
        }

        public bool Xoa(HT_SO_TT obj)
        {
            bool kq = true;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    entities.DeleteObject(obj);
                    entities.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;
        }

        public HT_SO_TT GetByID(string maPhanHe, string giaTriThanhPhan)
        {
            HT_SO_TT kq = null;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    kq = entities.HT_SO_TT.FirstOrDefault(e => e.MA_PHAN_HE.Equals(maPhanHe) && e.GTRI_TPHAN.Equals(giaTriThanhPhan));
                }
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;
        }

        public bool ThemTheoList(List<HT_SO_TT> lst)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                foreach (HT_SO_TT obj in lst)
                {
                    entities.HT_SO_TT.AddObject(obj);
                }
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
    }
}
