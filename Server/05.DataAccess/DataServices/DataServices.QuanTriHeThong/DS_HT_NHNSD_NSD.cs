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
    public class DS_HT_NHNSD_NSD
    {
        private string ENTITY_SET_NAME = "HT_NHNSD_NSD";

        public HT_NHNSD_NSD Them(HT_NHNSD_NSD obj)
        {
            HT_NHNSD_NSD kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NHNSD_NSD.AddObject(obj);
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

        public HT_NHNSD_NSD Sua(HT_NHNSD_NSD obj)
        {
            HT_NHNSD_NSD kq = null;            
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

        public bool Xoa(HT_NHNSD_NSD obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NHNSD_NSD.DeleteObject(obj);
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
                        HT_NHNSD_NSD obj = new HT_NHNSD_NSD();
                        obj = entities.HT_NHNSD_NSD.FirstOrDefault(e => e.ID == id);
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
                throw ex;
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public bool XoaTheoID_NHNSD(int ID, BusinessConstant.LoaiDoiTuong loaiDoiTuong)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    List<HT_NHNSD_NSD> lst = new List<HT_NHNSD_NSD>();
                    if (loaiDoiTuong == BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG)
                        lst = entities.HT_NHNSD_NSD.Where(e => e.ID_NSD == ID).ToList();
                    if (loaiDoiTuong == BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG)
                        lst = entities.HT_NHNSD_NSD.Where(e => e.ID_NHNSD == ID).ToList();
                    foreach (HT_NHNSD_NSD obj in lst)
                    {
                        entities.HT_NHNSD_NSD.Attach(obj);
                        entities.HT_NHNSD_NSD.DeleteObject(obj);
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
                        HT_NHNSD_NSD obj = new HT_NHNSD_NSD();
                        obj = entities.HT_NHNSD_NSD.FirstOrDefault(e => e.ID == id);
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
                        HT_NHNSD_NSD obj = new HT_NHNSD_NSD();
                        obj = entities.HT_NHNSD_NSD.FirstOrDefault(e => e.ID == id);
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

        public bool ThoaiDuyetListTheoID(List<int> listID)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (int id in listID)
                    {
                        HT_NHNSD_NSD obj = new HT_NHNSD_NSD();
                        obj = entities.HT_NHNSD_NSD.FirstOrDefault(e => e.ID == id);
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

        public HT_NHNSD_NSD GetById(int id)
        {
            HT_NHNSD_NSD kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_NHNSD_NSD.FirstOrDefault(e => e.ID == id);
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

        public List<HT_NHNSD_NSD> GetByIdNHNSD(int id_NHNSD)
        {
            List<HT_NHNSD_NSD> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_NHNSD_NSD.Where(e => e.ID_NHNSD == id_NHNSD).ToList();
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

        public List<HT_NHNSD_NSD> GetByIdNSD(int id_NSD)
        {
            List<HT_NHNSD_NSD> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_NHNSD_NSD.Where(e => e.ID_NSD == id_NSD).ToList();
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
