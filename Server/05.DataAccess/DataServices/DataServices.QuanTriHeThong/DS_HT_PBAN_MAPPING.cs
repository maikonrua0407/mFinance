using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    public class DS_HT_PBAN_MAPPING
    {
        private string ENTITY_SET_NAME = "HT_PBAN_MAPPING";

        public List<HT_PBAN_MAPPING> GetHtPbanMappingByServerVersion(string serverVersion)
        {
            List<HT_PBAN_MAPPING> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_PBAN_MAPPING.Where(e => e.MA_PBAN_SERVER == serverVersion).OrderBy(e => e.ID).ToList();
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
    }
}
