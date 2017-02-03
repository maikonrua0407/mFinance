using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Transactions;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    /// <summary>
    /// Lịch làm việc, lịch nghỉ
    /// </summary>
    public class DS_HT_LICH
    {
        private const string ENTITY_SET_NAME = "HT_LICH";

        /// <summary>
        /// Lấy nội dung bảng HT_LICH
        /// </summary>
        /// <returns>Trả lại danh sách toàn bộ các dòng dữ liệu</returns>
        public List<HT_LICH> LayLich()
        {
            List<HT_LICH> htLich = new List<HT_LICH>();
            Entities entities = ContextFactory.GetInstance();
            htLich = entities.HT_LICH.Where(lich => true).ToList();
            // using (var ctx = new Entities())
            //using (var ctx = ContextFactory.GetInstance())
            //{ 
            //    // Lấy tất cả các dòng là lịch nghỉ, lễ hằng năm và ngày nghỉ/làm việc cụ thể nếu có
            //    //htLich = ctx.HT_LICH.AsQueryable().Select(lich => lich).ToList();
            //    htLich = ctx.HT_LICH.Select(lich => lich).ToList();
            //}
            return htLich;
        }
        
        /// <summary>
        /// Lấy nội dung bảng HT_LICH
        /// </summary>
        /// <param name="ngay">Chuỗi chứa ngày cần giới hạn theo định dạng chuẩn</param>
        /// <returns>Trả lại danh sách các dòng dữ liệu</returns>
        public List<HT_LICH> LayLich(string ngay)
        {
            List<HT_LICH> htLich = new List<HT_LICH>();
            Entities entities = ContextFactory.GetInstance();
            htLich = entities.HT_LICH.Where(lich => lich.DINH_DANG != ApplicationConstant.defaultDateTimeFormat || lich.GIA_TRI.Equals(ngay)).ToList();
            // using (var ctx = new Entities())
            //using (var ctx = ContextFactory.GetInstance())
            //{ 
            //    // Lấy các dòng là lịch nghỉ, lễ hằng năm và ngày nghỉ/làm việc cụ thể nếu có
            //    htLich = ctx.HT_LICH.Where(lich => lich.DINH_DANG != ApplicationConstant.defaultDateTimeFormat || lich.GIA_TRI == ngay).ToList();
            //}
            return htLich;
        }

        /// <summary>
        /// Lấy nội dung bảng HT_LICH
        /// </summary>
        /// <param name="year">Năm cần giới hạn</param>
        /// <param name="month">Tháng cần giới hạn</param>
        /// <returns>Trả lại danh sách các dòng dữ liệu lịch</returns>
        public List<HT_LICH> LayLich(int year, int month)
        {
            List<HT_LICH> htLich = new List<HT_LICH>();
            Entities entities = ContextFactory.GetInstance();
            htLich = entities.HT_LICH.Where(lich => lich.DINH_DANG != ApplicationConstant.defaultDateTimeFormat
                                               || (lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) >= LDateTime.GetFirstDateOfMonth(year, month)
                                                && lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) <= LDateTime.GetLastDateOfMonth(year, month))).ToList();
            // using (var ctx = new Entities())
            //using (var ctx = ContextFactory.GetInstance())
            //{ 
            //    // Lấy các dòng là lịch nghỉ, lễ hằng năm và ngày nghỉ/làm việc trong tháng nếu có
            //    //htLich = ctx.HT_LICH.AsEnumerable().Where(lich => lich.DINH_DANG != ApplicationConstant.defaultDateTimeFormat
            //    //                               || (lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) >= LDateTime.GetFirstDateOfMonth(year, month)
            //    //                                && lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) <= LDateTime.GetLastDateOfMonth(year, month))).ToList();
            //    htLich = ctx.HT_LICH.Where(lich => lich.DINH_DANG != ApplicationConstant.defaultDateTimeFormat
            //                                   || (lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) >= LDateTime.GetFirstDateOfMonth(year, month)
            //                                    && lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) <= LDateTime.GetLastDateOfMonth(year, month))).ToList();
            //}
            return htLich;
        }

        /// <summary>
        /// Cập nhật toàn bộ bảng HT_LICH
        /// </summary>
        /// <param name="value">Danh sách các object dữ liệu lịch</param>
        public void LuuLich(List<HT_LICH> value)
        {
            object original;
            //using (var ctx = new Entities())
            using (var ctx = ContextFactory.GetInstance())
            {
                foreach (HT_LICH htLich in value)
                {
                    EntityKey key = ctx.CreateEntityKey(ENTITY_SET_NAME, htLich);
                    if (ctx.TryGetObjectByKey(key, out original)) ctx.ApplyCurrentValues(key.EntitySetName, htLich);
                    else ctx.HT_LICH.AddObject(htLich);
                }
                ctx.SaveChanges();
            }
        }

        public bool Them(HT_LICH obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_LICH.AddObject(obj);
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

        public bool Sua(HT_LICH obj)
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

        public bool Xoa(HT_LICH obj)
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
                        HT_LICH obj = new HT_LICH();
                        obj = entities.HT_LICH.FirstOrDefault(e => e.ID == id);
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
                        HT_LICH obj = new HT_LICH();
                        obj = entities.HT_LICH.FirstOrDefault(e => e.ID == id);
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
                        HT_LICH obj = new HT_LICH();
                        obj = entities.HT_LICH.FirstOrDefault(e => e.ID == id);
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
                        HT_LICH obj = new HT_LICH();
                        obj = entities.HT_LICH.FirstOrDefault(e => e.ID == id);
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

        public HT_LICH GetById(int id)
        {
            HT_LICH kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_LICH.FirstOrDefault(e => e.ID == id);
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

        public HT_LICH GetByMa(string ma)
        {
            HT_LICH kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                kq = entities.HT_LICH.FirstOrDefault(e => e.MA_LICH.Equals(ma));
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
