using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.ZAMainApp
{
    public class Login
    {
        public HT_NSD doLogin(string userName, string passWord)
        {
            HT_NSD htNguoiDung = null;

            // Mã hóa mật khẩu
            string passWordEncoding = passWord;

            try
            {
                htNguoiDung = GetNguoiDung(userName, passWord);
                return htNguoiDung;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogLevel.Error, ex);
            }
            return htNguoiDung;
        }

        public HT_NSD GetNguoiDung(string userName, string passWord)
        {
            HT_NSD htNguoiDungTemp = null;
            try
            {
                //mFinanceCoreEntities context = ContextFactory.GetInstance();
                //mFinanceCoreEntities context = new mFinanceCoreEntities();
                var context = new Entities();
                htNguoiDungTemp = context.HT_NSD.FirstOrDefault(e => e.MA_DANG_NHAP == userName && e.MAT_KHAU == passWord);

                if (htNguoiDungTemp != null)
                {
                    return htNguoiDungTemp;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogLevel.Error, ex);
            }
            return htNguoiDungTemp;
        }
    }
}
