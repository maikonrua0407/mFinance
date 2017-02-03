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
    public class DS_HT_NSD
    {
		private string ENTITY_SET_NAME = "HT_NSD";

        public HT_NSD Them(HT_NSD obj)
        {
            HT_NSD kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NSD.AddObject(obj);
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

        public HT_NSD Sua(HT_NSD obj)
        {
            HT_NSD kq = null;            
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

        public bool Xoa(HT_NSD obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NSD.Attach(obj);
                entities.HT_NSD.DeleteObject(obj);
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
                        HT_NSD obj = new HT_NSD();
                        obj = entities.HT_NSD.FirstOrDefault(e => e.ID == id);
                        entities.HT_NSD.DeleteObject(obj);
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
                        HT_NSD obj = new HT_NSD();
                        obj = entities.HT_NSD.FirstOrDefault(e => e.ID == id);
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
                throw ex;
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
                        HT_NSD obj = new HT_NSD();
                        obj = entities.HT_NSD.FirstOrDefault(e => e.ID == id);
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
                throw ex;
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
                        HT_NSD obj = new HT_NSD();
                        obj = entities.HT_NSD.FirstOrDefault(e => e.ID == id);
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
                throw ex;
            }
            finally
            {
                entities = null;
            }
            return kq;
        }

        public HT_NSD GetById(int id)
        {
            HT_NSD kq = null;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    kq = entities.HT_NSD.FirstOrDefault(e => e.ID == id);
                    return kq;
                }
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }        

        public HT_NSD GetByMa(string ma)
        {
            HT_NSD kq = null;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    kq = entities.HT_NSD.FirstOrDefault(e => e.MA_NSD.Equals(ma));
                    return kq;
                }
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public HT_NSD GetByTenDangNhap(string tenDangNhap)
        {
            HT_NSD kq = null;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    kq = entities.HT_NSD.FirstOrDefault(e => e.MA_DANG_NHAP.Equals(tenDangNhap));
                    return kq;
                }
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_NSD> GetByListId(List<int> listID)
        {
            List<HT_NSD> kq = null;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    kq = entities.HT_NSD.Where(e => listID.Contains(e.ID)).ToList();
                    return kq;
                }
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Lấy người dùng trong đơn vị, ngoại trừ nguồn HTH
        /// </summary>
        /// <param name="maDonVi"></param>
        /// <returns></returns>
        public List<HT_NSD> GetListByDonVi(string maDonVi)
        {
            List<HT_NSD> kq = null;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    string nguonHeThong = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                    kq = entities.HT_NSD.Where(e => e.MA_DVI_QLY.Equals(maDonVi) && !e.NGUON_TAO_DL.Equals(nguonHeThong)).ToList();
                    return kq;
                }
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tất cả người dùng trong hệ thống, trừ nguồn HTH
        /// </summary>
        /// <returns></returns>
        public List<HT_NSD> GetAll()
        {
            List<HT_NSD> kq = null;
            
            try
            {
                using (Entities entities = ContextFactory.GetInstance())
                {
                    string nguonHeThong = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                    kq = entities.HT_NSD.Where(e => !(e.NGUON_TAO_DL.Equals(nguonHeThong))).ToList();
                    return kq;
                }
            }
            catch (System.Exception ex)
            {
                kq = null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
