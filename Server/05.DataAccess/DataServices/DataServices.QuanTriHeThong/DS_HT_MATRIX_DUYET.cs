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
    public class DS_HT_MATRIX_DUYET
    {
        private string ENTITY_SET_NAME = "HT_MATRIX_DUYET";

        public bool Them(HT_MATRIX_DUYET obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_MATRIX_DUYET.AddObject(obj);
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

        public bool Sua(HT_MATRIX_DUYET obj)
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

        public bool Xoa(HT_MATRIX_DUYET obj)
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
                        HT_MATRIX_DUYET obj = new HT_MATRIX_DUYET();
                        obj = entities.HT_MATRIX_DUYET.FirstOrDefault(e => e.ID == id);
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
                        HT_MATRIX_DUYET obj = new HT_MATRIX_DUYET();
                        obj = entities.HT_MATRIX_DUYET.FirstOrDefault(e => e.ID == id);
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
                        HT_MATRIX_DUYET obj = new HT_MATRIX_DUYET();
                        obj = entities.HT_MATRIX_DUYET.FirstOrDefault(e => e.ID == id);
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

        public bool HuyDuyetListTheoID(List<int> listID)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (int id in listID)
                    {
                        HT_MATRIX_DUYET obj = new HT_MATRIX_DUYET();
                        obj = entities.HT_MATRIX_DUYET.FirstOrDefault(e => e.ID == id);
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

        public HT_MATRIX_DUYET GetById(int id)
        {
            HT_MATRIX_DUYET kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_MATRIX_DUYET.FirstOrDefault(e => e.ID == id);
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

        public HT_MATRIX_DUYET GetByMa(string loaiDTuong,string loaiDuLieu, string maDuLieu, string maDoiTuong, string maChucNang)
        {
            HT_MATRIX_DUYET kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                kq = entities.HT_MATRIX_DUYET.FirstOrDefault(e => e.LOAI_DTUONG.Equals(loaiDTuong) 
                    && e.MA_DULIEU.Equals(maDoiTuong) 
                    && e.MA_DTUONG.Equals(maDoiTuong) 
                    && e.MA_CNANG.Equals(maChucNang)
                    && e.LOAI_DULIEU.Equals(loaiDuLieu));
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

        public HT_MATRIX_DUYET GetByMa(string loaiDTuong, string maDuLieu, string maDoiTuong, string maChucNang)
        {
            HT_MATRIX_DUYET kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_MATRIX_DUYET.OrderByDescending(f=>f.ID).
                    FirstOrDefault(e => e.LOAI_DTUONG.Equals(loaiDTuong) && 
                        e.MA_DULIEU.Equals(maDoiTuong) && e.MA_DTUONG.Equals(maDoiTuong) 
                        && e.MA_CNANG.Equals(maChucNang));
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

        public HT_MATRIX_DUYET GetByMa(string loaiDuLieu, string maDuLieu, string maChucNang)
        {
            HT_MATRIX_DUYET kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_MATRIX_DUYET.OrderByDescending(f => f.ID).
                    FirstOrDefault(e => e.LOAI_DULIEU.Equals(loaiDuLieu) 
                        && e.MA_DULIEU.Equals(maDuLieu)
                        && e.MA_CNANG.Equals(maChucNang));
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
