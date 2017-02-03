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
    public class DS_HT_NHNSD
    {
        private string ENTITY_SET_NAME = "HT_NHNSD";

        /// <summary>
        /// Tạo mã khách hàng
        /// </summary>
        /// <param name="loaiKH"></param>
        /// <param name="ngayThamGia"></param>
        /// <returns></returns>
        private string TaoMaNhomNSD()
        {
            DataServices.Utilities.DS_SinhMa process = new DataServices.Utilities.DS_SinhMa();
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@TenBang", "STRING", DatabaseConstant.Table.HT_NHNSD.getValue());
            LDatatable.AddParameter(ref dt, "@TenTruong", "STRING", "MA_NHNSD");
            return process.TaoMa("MA_NHNSD", dt);
        }

        public HT_NHNSD Them(HT_NHNSD obj)
        {
            HT_NHNSD kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NHNSD.AddObject(obj);
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

        public HT_NHNSD Sua(HT_NHNSD obj)
        {
            HT_NHNSD kq = null;            
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

        public bool Xoa(HT_NHNSD obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_NHNSD.Attach(obj);
                entities.HT_NHNSD.DeleteObject(obj);
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
                        HT_NHNSD obj = new HT_NHNSD();
                        obj = entities.HT_NHNSD.FirstOrDefault(e => e.ID == id);
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
                        HT_NHNSD obj = new HT_NHNSD();
                        obj = entities.HT_NHNSD.FirstOrDefault(e => e.ID == id);
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
                        HT_NHNSD obj = new HT_NHNSD();
                        obj = entities.HT_NHNSD.FirstOrDefault(e => e.ID == id);
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
                        HT_NHNSD obj = new HT_NHNSD();
                        obj = entities.HT_NHNSD.FirstOrDefault(e => e.ID == id);
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

        public HT_NHNSD GetById(int id)
        {
            HT_NHNSD kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_NHNSD.FirstOrDefault(e => e.ID == id);
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

        public HT_NHNSD GetByMa(string ma)
        {
            HT_NHNSD kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                kq = entities.HT_NHNSD.FirstOrDefault(e => e.MA_NHNSD == ma);
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

        public List<HT_NHNSD> GetByListId(List<int> listID)
        {
            List<HT_NHNSD> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_NHNSD.Where(e => listID.Contains(e.ID)).ToList();
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

        public List<HT_NHNSD> GetListByDonVi(string maDonVi)
        {
            List<HT_NHNSD> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                string nguonHeThong = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                kq = entities.HT_NHNSD.Where(e => e.MA_DVI_QLY.Equals(maDonVi) && !e.NGUON_TAO_DL.Equals(nguonHeThong)).ToList();
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

        public List<HT_NHNSD> GetAll()
        {
            List<HT_NHNSD> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                string nguonHeThong = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                kq = entities.HT_NHNSD.Where(e => !e.NGUON_TAO_DL.Equals(nguonHeThong)).ToList();
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

        public List<HT_NHNSD> GetByIdNSD(int id_NSD)
        {
            List<HT_NHNSD> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                var query = from a in entities.HT_NHNSD
                            join b in entities.HT_NHNSD_NSD on a.ID equals b.ID_NHNSD
                            where b.ID_NSD == id_NSD
                            select a;
                kq = query.ToList();
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
