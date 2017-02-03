using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using Utilities.Common;
using System.Data;

namespace DataServices.QuanTriHeThong
{
    public class DS_HT_PHAM_VI
    {

        private string ENTITY_SET_NAME = "HT_PHAM_VI";

        public bool Them(HT_PHAM_VI obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_PHAM_VI.AddObject(obj);
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

        public bool Sua(HT_PHAM_VI obj)
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

        public bool Xoa(HT_PHAM_VI obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();

            try
            {
                entities.HT_PHAM_VI.Attach(obj);
                entities.HT_PHAM_VI.DeleteObject(obj);
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

        public HT_PHAM_VI GetById(int id)
        {
            HT_PHAM_VI kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                kq = entities.HT_PHAM_VI.FirstOrDefault(e => e.ID == id);
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

        public List<HT_PHAM_VI> GetPhamViDonVi_ByDoiTuong(string maLoaiDoiTuong, string maDoiTuong)
        {
            List<HT_PHAM_VI> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                string loaiPhamVi = BusinessConstant.LOAI_PHAM_VI.DON_VI.layGiaTri();
                kq = entities.HT_PHAM_VI.Where(e => e.MA_PVI_LOAI.Equals(loaiPhamVi) && e.MA_DTUONG_LOAI.Equals(maLoaiDoiTuong) && e.MA_DTUONG.Equals(maDoiTuong)).ToList();
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

        public List<HT_PHAM_VI> GetPhamViDiaLy_ByDoiTuong(string maLoaiDoiTuong, string maDoiTuong)
        {
            List<HT_PHAM_VI> kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                string loaiPhamVi = BusinessConstant.LOAI_PHAM_VI.DIA_LY.layGiaTri();
                kq = entities.HT_PHAM_VI.Where(e => e.MA_PVI_LOAI.Equals(loaiPhamVi) && e.MA_DTUONG_LOAI.Equals(maLoaiDoiTuong) && e.MA_DTUONG.Equals(maDoiTuong)).ToList();
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
                List<HT_PHAM_VI> lstCNangPQuyen = entities.HT_PHAM_VI.Where(e => e.ID_DTUONG == idDoiTuong && e.MA_DTUONG.Equals(maDoiTuong) && e.MA_DTUONG_LOAI.Equals(loaiDoiTuong)).ToList();

                foreach (HT_PHAM_VI item in lstCNangPQuyen)
                {
                    entities.HT_PHAM_VI.Attach(item);
                    entities.HT_PHAM_VI.DeleteObject(item);
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
