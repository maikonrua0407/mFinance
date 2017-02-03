using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using System.Data;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    public class DS_QuanTriHeThong
    {
        public List<HT_CNANG> LayChucNangTheoQuyen(string maDangNhap, string passWord)
        {
            try
            {
                DS_PhanQuyen pq = new DS_PhanQuyen();
                // Entities entities = new Entities();
                Entities entities = ContextFactory.GetInstance();
                DataSet ds = pq.LayTNuyenKThacTheoUser(maDangNhap, passWord);
                var ma=ds.Tables[0].AsEnumerable().Select(e=>e.Field<string>("MA_TNGUYEN"));
                var id = entities.HT_CNANG.Where(e => ma.Contains(e.MA_CNANG)).Select(e => e.ID);
                return entities.HT_CNANG.Where(e =>id.Contains(e.ID)).ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        public bool DoiMatKhau(string pv_strUserName, string pv_strPass)
        {
            bool v_blRet = true;

            try
            {
                NG.ADO.LDataAccessLayer DAL = new NG.ADO.LDataAccessLayer();
                DAL.ExecuteNonQuery("exec wsp_HT_NSD_DoiMatkhau '" + pv_strUserName + "','" + pv_strPass + "'");

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                v_blRet = false;
            }

            return v_blRet;
        }

        public List<HT_NSD> layNSD()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_NSD.ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        public List<HT_NHNSD> layNhomNSD()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_NHNSD.ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        public List<HT_TSO_LOAI> layLoaiThamSo()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_TSO_LOAI.ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        public List<HT_TSO> layThamSo()
        {
            try
            {
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_TSO.ToList();
            }
            catch (Exception ex)
            {
                // Ghi log
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }
    }
}
