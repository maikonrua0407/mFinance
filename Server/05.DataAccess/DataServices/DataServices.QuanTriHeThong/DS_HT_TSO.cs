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
    public class DS_HT_TSO
    {
		private string ENTITY_SET_NAME = "HT_TSO";

        /// <summary>
        /// Thêm đối tượng Tham số
        /// </summary>
        /// <param name="obj">Đối tượng Tham số</param>
        /// <returns>True nếu thành công, false ngược lại</returns>
        public bool Them(HT_TSO obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_TSO.AddObject(obj);
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

        public bool Sua(HT_TSO obj)
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

        public HT_TSO SuaGiaTriThamSo(HT_TSO obj)
        {
            HT_TSO kq = null;
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

        public bool Xoa(HT_TSO obj)
        {
            bool kq = true;
            Entities entities = ContextFactory.GetInstance();
            try
            {
                entities.HT_TSO.Attach(obj);
                entities.HT_TSO.DeleteObject(obj);
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
                        HT_TSO obj = new HT_TSO();
                        obj = entities.HT_TSO.FirstOrDefault(e => e.ID == id);
                        entities.HT_TSO.DeleteObject(obj);
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
                        HT_TSO obj = new HT_TSO();
                        obj = entities.HT_TSO.FirstOrDefault(e => e.ID == id);
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
                        HT_TSO obj = new HT_TSO();
                        obj = entities.HT_TSO.FirstOrDefault(e => e.ID == id);
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
                        HT_TSO obj = new HT_TSO();
                        obj = entities.HT_TSO.FirstOrDefault(e => e.ID == id);
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

        public HT_TSO GetById(int id)
        {
            HT_TSO kq = null;
            Entities entities = ContextFactory.GetInstance();                
            try
            {
                kq = entities.HT_TSO.FirstOrDefault(e => e.ID == id);
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
            return kq;
        }

        public HT_TSO GetByMa(string ma)
        {
            HT_TSO kq = null;
            Entities entities = ContextFactory.GetInstance();
            try
            {                
                kq = entities.HT_TSO.FirstOrDefault(e => e.MA_TSO.Equals(ma));
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
            return kq;
        }

        public List<HT_TSO> layThamSoTheoLoaiThamSo(string loaiThamSo)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_TSO.Where(e => e.MA_TSO_LOAI.Equals(loaiThamSo)).ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TSO> layThamSoDonVi(string maDonVi)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                string strLoaiThamSo = BusinessConstant.LoaiThamSo.DV.layGiaTri();
                return entities.HT_TSO.Where(e => e.MA_DVI_QLY.Equals(maDonVi) && e.MA_TSO_LOAI.Equals(strLoaiThamSo)).ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TSO> layThamSoDonVi()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                string strLoaiThamSo = BusinessConstant.LoaiThamSo.DV.layGiaTri();
                string strPhamViThamSo = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                string strNguonThamSo = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                return entities.HT_TSO.Where(e => e.MA_TSO_LOAI.Equals(strLoaiThamSo) && e.PVI_AHUONG.Equals(strPhamViThamSo) && e.NGUON_TAO_DL.Equals(strNguonThamSo)).ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TSO> layThamSoTrungUong()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                string strLoaiThamSo = BusinessConstant.LoaiThamSo.TW.layGiaTri();
                string strPhamViThamSo = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                string strNguonThamSo = DatabaseConstant.NguonTaoDuLieu.HTH.layGiaTri();
                return entities.HT_TSO.Where(e => e.MA_TSO_LOAI.Equals(strLoaiThamSo) && e.PVI_AHUONG.Equals(strPhamViThamSo) && e.NGUON_DL.Equals(strNguonThamSo)).ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public List<HT_TSO> layThamSoDonViTheoLoaiThamSo(string maDonVi, string loaiThamSo)
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_TSO.Where(e => e.MA_DVI_QLY.Equals(maDonVi) && e.MA_TSO_LOAI.Equals(loaiThamSo)).ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public HT_TSO layThamSoChung(BusinessConstant.LoaiThamSo loaiThamSo, BusinessConstant.MaThamSo maThamSo)
        {
            try
            {
                HT_TSO htTso = null;
                Entities entities = ContextFactory.GetInstance();
                string strLoaiThamSo = loaiThamSo.layGiaTri();
                string strMaThamSo = maThamSo.layGiaTri();
                //htTso = entities.HT_TSO.FirstOrDefault(e => e.MA_TSO_LOAI.Equals(strLoaiThamSo) && e.MA_TSO.Equals(strMaThamSo));
                htTso = entities.HT_TSO.Where(e => e.MA_TSO_LOAI.Equals(strLoaiThamSo) && e.MA_TSO.Equals(strMaThamSo)).ToList().FirstOrDefault();
                return htTso;
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public HT_TSO layThamSoDonVi(BusinessConstant.LoaiThamSo loaiThamSo, BusinessConstant.MaThamSo maThamSo, string maDonVi)
        {
            try
            {
                HT_TSO htTso = null;
                Entities entities = ContextFactory.GetInstance();
                string strLoaiThamSo = loaiThamSo.layGiaTri();
                string strMaThamSo = maThamSo.layGiaTri();
                //htTso = entities.HT_TSO.FirstOrDefault(e => e.MA_TSO_LOAI.Equals(strLoaiThamSo) && e.MA_TSO.Equals(strMaThamSo) && e.MA_DVI_QLY.Equals(maDonVi));
                htTso = entities.HT_TSO.Where(e => e.MA_TSO_LOAI.Equals(strLoaiThamSo) && e.MA_TSO.Equals(strMaThamSo) && e.MA_DVI_QLY.Equals(maDonVi)).ToList().FirstOrDefault();
                return htTso;
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
