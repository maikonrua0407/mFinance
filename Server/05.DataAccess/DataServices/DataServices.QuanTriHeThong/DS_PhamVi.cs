using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG.ADO;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    public class DS_PhamVi
    {
        public DataSet LayDanhSachPhamViDuLieu(string maDangNhap, string maDonVi)
        {
            string sql = "";
            try
            {
                // Kiểm tra timing
                LDataAccessLayer DAL = new LDataAccessLayer();
                sql = @"EXEC	[dbo].[sp_INQ.DS.QTHT_PhamViDuLieu_DonVi] '" + maDangNhap + "','" + maDonVi + "'";
                DataSet ds = DAL.DataSetExecuteADO(sql);
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
