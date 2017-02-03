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
    public class DS_NN_RESOURCE
    {
        private string ENTITY_SET_NAME = "NN_RESOURCE";

        public List<NN_RESOURCE> GetAll()
        {
            List<NN_RESOURCE> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.NN_RESOURCE.ToList();
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

        public NN_RESOURCE GetNgonNguByKey(string key)
        {
            NN_RESOURCE ngonNgu = null;
            try
            {
                Entities entities = ContextFactory.GetInstance();
                ngonNgu = entities.NN_RESOURCE.FirstOrDefault(e => e.MA.Equals(key));

                return ngonNgu;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }        
    }
}
