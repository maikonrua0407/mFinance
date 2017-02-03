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
    public class DS_HT_CNANG_PQUYEN
    {
        private string ENTITY_SET_NAME = "HT_CNANG_PQUYEN";

        public bool Them(HT_CNANG_PQUYEN obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_CNANG_PQUYEN.AddObject(obj);
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

        public bool Sua(HT_CNANG_PQUYEN obj)
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
                throw ex;
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool Xoa(HT_CNANG_PQUYEN obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();

            try
            {
                entities.HT_CNANG_PQUYEN.Attach(obj);
                entities.HT_CNANG_PQUYEN.DeleteObject(obj);
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

        public HT_CNANG_PQUYEN GetById(int id)
        {
            HT_CNANG_PQUYEN kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_CNANG_PQUYEN.FirstOrDefault(e => e.ID == id);
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

        public bool XoaTheoDoiTuong(int idDoiTuong, string maDoiTuong, string loaiDoiTuong)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                List<HT_CNANG_PQUYEN> lstCNangPQuyen = entities.HT_CNANG_PQUYEN.Where(e => e.ID_DTUONG == idDoiTuong && e.MA_DTUONG.Equals(maDoiTuong) && e.MA_DTUONG_LOAI.Equals(loaiDoiTuong)).ToList();

                foreach (HT_CNANG_PQUYEN item in lstCNangPQuyen)
                {
                    entities.HT_CNANG_PQUYEN.Attach(item);
                    entities.HT_CNANG_PQUYEN.DeleteObject(item);
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
