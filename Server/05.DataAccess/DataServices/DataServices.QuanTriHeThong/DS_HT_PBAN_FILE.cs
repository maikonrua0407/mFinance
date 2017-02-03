using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    public class DS_HT_PBAN_FILE
    {
        private string ENTITY_SET_NAME = "HT_PBAN_FILE";
        public List<HT_PBAN_FILE> GetListByMaPban(string ma)
        {
            List<HT_PBAN_FILE> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                var query = from file in entities.HT_PBAN_FILE
                            join ctiet in entities.HT_PBAN_CTIET on file.MA_FILE equals ctiet.MA_FILE
                            where ctiet.MA_PBAN.Equals(ma)
                            select file;

                kq = query.OrderBy(e => e.MA_FILE).ToList<HT_PBAN_FILE>();
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
