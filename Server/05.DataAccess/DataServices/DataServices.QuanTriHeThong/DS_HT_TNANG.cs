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
    public class DS_HT_TNANG
    {
        /// <summary>
        /// Lấy thông tin tính năng theo tài nguyên khai thác của NSD
        /// </summary>
        /// <param name="maDangNhap">mã đăng nhập</param>
        /// <param name="maDonVi">mã đơn vị quản lý</param>
        /// <param name="maTaiNguyen">mã tài nguyên</param>
        /// <param name="loaiTaiNguyen">mã loại tài nguyên</param>
        /// <returns></returns>
        public List<HT_TNANG> LayTNangKThac(string maDangNhap, string maDonVi, string maTaiNguyen, string loaiTaiNguyen)
        {
            // Entities entities = new Entities();
            Entities entities = ContextFactory.GetInstance();
            // Lấy ID NSD
            var id_NSD = entities.HT_NSD.FirstOrDefault(e => e.MA_DANG_NHAP.Equals(maDangNhap) && e.MA_DVI_QLY.Equals(maDonVi)).ID;
            var maTNguyen = entities.HT_TNGUYEN.Where(e => e.GTRI_TNGUYEN.Equals(maTaiNguyen)&&e.MA_TNGUYEN_LOAI.Equals(loaiTaiNguyen)).Select(e=>e.MA_TNGUYEN);
            DS_HT_TNGUYEN_KTHAC dsTNGUYENKTHAC = new DS_HT_TNGUYEN_KTHAC();
            var TNKT = dsTNGUYENKTHAC.LayTNguyenKThacNSD(id_NSD, maDonVi).Where(e=>maTNguyen.Contains(e.MA_TNGUYEN_CHA)).Select(e => e.MA_TNGUYEN);

            // Lấy ds Tài nguyên
            var taiNguyen = entities.HT_TNGUYEN.Where(e => TNKT.Contains(e.MA_TNGUYEN)).Distinct().Select(e => e.GTRI_TNGUYEN);
            return entities.HT_TNANG.Where(e => taiNguyen.Contains(e.MA_TNANG)).ToList();
        }

		private string ENTITY_SET_NAME = "HT_TNANG";

        public bool Them(HT_TNANG obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_TNANG.AddObject(obj);
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

        public bool Sua(HT_TNANG obj)
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

        public bool Xoa(HT_TNANG obj)
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
                        HT_TNANG obj = new HT_TNANG();
                        obj = entities.HT_TNANG.FirstOrDefault(e => e.ID == id);
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
                        HT_TNANG obj = new HT_TNANG();
                        obj = entities.HT_TNANG.FirstOrDefault(e => e.ID == id);
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
                        HT_TNANG obj = new HT_TNANG();
                        obj = entities.HT_TNANG.FirstOrDefault(e => e.ID == id);
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
                        HT_TNANG obj = new HT_TNANG();
                        obj = entities.HT_TNANG.FirstOrDefault(e => e.ID == id);
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

        public HT_TNANG GetById(int id)
        {
            HT_TNANG kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_TNANG.FirstOrDefault(e => e.ID == id);
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

        public HT_TNANG GetByMa(string ma)
        {
            HT_TNANG kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                kq = entities.HT_TNANG.FirstOrDefault(e => e.MA_TNANG.Equals(ma));
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
