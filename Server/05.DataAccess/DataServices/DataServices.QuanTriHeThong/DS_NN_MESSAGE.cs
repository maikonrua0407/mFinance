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
    public class DS_NN_MESSAGE
    {
        private string ENTITY_SET_NAME = "NN_MESSAGE";

        public List<NN_MESSAGE> GetAll()
        {
            List<NN_MESSAGE> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.NN_MESSAGE.ToList();
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

        public NN_MESSAGE GetMessageByKey(string key)
        {
            NN_MESSAGE message = null;
            try
            {
                Entities entities = ContextFactory.GetInstance();
                message = entities.NN_MESSAGE.FirstOrDefault(e => e.MA.Equals(key));

                return message;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
