using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using DataServices.TruyVan;
using Utilities.Common;
using System.Data;

namespace DataServices.ZAMainApp
{
    public class DS_ZAMainApp
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
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public HT_NSD GetNguoiDung(string userName, string passWord)
        {
            HT_NSD htNguoiDungTemp = null;
            try
            {
                Entities entities = ContextFactory.GetInstance();
                htNguoiDungTemp = entities.HT_NSD.FirstOrDefault(e => e.MA_DANG_NHAP.Equals(userName) && e.MAT_KHAU.Equals(passWord));
                //htNguoiDungTemp = entities.HT_NSD.Where(e => e.MA_DANG_NHAP == userName && e.MAT_KHAU == passWord).First();

                return htNguoiDungTemp;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public HT_NSD GetNguoiDung(string userName)
        {
            HT_NSD htNguoiDungTemp = null;
            try
            {
                Entities entities = ContextFactory.GetInstance();
                htNguoiDungTemp = entities.HT_NSD.FirstOrDefault(e => e.MA_DANG_NHAP.Equals(userName));
                //htNguoiDungTemp = entities.HT_NSD.Where(e => e.MA_DANG_NHAP == userName && e.MAT_KHAU == passWord).First();

                return htNguoiDungTemp;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public int CapNhatKheUocDaTatToan(ApplicationConstant.DonViSuDung CompanyCode)
        {
            int SoLuongKheUoc = 0;
            try
            {
                if (CompanyCode.Equals(ApplicationConstant.DonViSuDung.BINHKHANH))
                {
                    string nativeQuery = "";
                    DS_TruyVan dsTruyVan = new DS_TruyVan();
                    DataSet ds = new DataSet();

                    // Lay so luong trong TD_KUOCVM
                    nativeQuery = "SELECT COUNT(*) FROM TD_KUOCVM WHERE SO_DU > 0 AND TTHAI_KUOC = 'DTT'";
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                    ds = dsTruyVan.GetData(nativeQuery);
                    string SoLuongKUOCVM = "";
                    if (ds != null && ds.Tables[0].Rows.Count > 0) SoLuongKUOCVM = ds.Tables[0].Rows[0][0].ToString();
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "KUOCVM: " + SoLuongKUOCVM);

                    // Lay so luong trong TD_KUOCVM_LSU
                    nativeQuery = "SELECT COUNT(*) FROM TD_KUOCVM_LSU WHERE SO_DU > 0 AND TTHAI_KUOC = 'DTT'";
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                    ds = dsTruyVan.GetData(nativeQuery);
                    string SoLuongKUOCVM_LSU = "";
                    if (ds != null && ds.Tables[0].Rows.Count > 0) SoLuongKUOCVM_LSU = ds.Tables[0].Rows[0][0].ToString();
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "KUOCVM_LSU: " + SoLuongKUOCVM_LSU);

                    if (SoLuongKUOCVM.Equals(SoLuongKUOCVM_LSU) && !SoLuongKUOCVM.Equals("0"))
                    {
                        SoLuongKheUoc = Int32.Parse(SoLuongKUOCVM);

                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "KUOCVM PROCESSING");
                        nativeQuery = "SELECT MA_KUOCVM, SO_DU, SO_TIEN_GIAI_NGAN, GOC_DA_THU, LAI_PHAI_THU, LAI_DA_THU FROM TD_KUOCVM WHERE SO_DU > 0 AND TTHAI_KUOC = 'DTT'";
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                        ds = dsTruyVan.GetData(nativeQuery);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string MA_KUOCVM = ds.Tables[0].Rows[i][0].ToString();
                                string SO_DU = ds.Tables[0].Rows[i][1].ToString();
                                string SO_TIEN_GIAI_NGAN = ds.Tables[0].Rows[i][2].ToString();
                                string GOC_DA_THU = ds.Tables[0].Rows[i][3].ToString();
                                string LAI_PHAI_THU = ds.Tables[0].Rows[i][4].ToString();
                                string LAI_DA_THU = ds.Tables[0].Rows[i][5].ToString();
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "MA_KUOCVM, SO_DU, SO_TIEN_GIAI_NGAN, GOC_DA_THU, LAI_PHAI_THU, LAI_DA_THU");
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, MA_KUOCVM + ", " + SO_DU + ", "+ SO_TIEN_GIAI_NGAN + ", " + GOC_DA_THU + ", " + LAI_PHAI_THU + ", " + LAI_DA_THU);
                            }
                            nativeQuery = "UPDATE TD_KUOCVM SET SO_DU = 0 WHERE SO_DU > 0 AND TTHAI_KUOC = 'DTT'";
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                            ds = dsTruyVan.GetData(nativeQuery);
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, ds.ToString());
                        }


                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "KUOCVM_LSU PROCESSING");
                        nativeQuery = "SELECT MA_KUOCVM, SO_DU, SO_TIEN_GIAI_NGAN, GOC_DA_THU, LAI_PHAI_THU, LAI_DA_THU FROM TD_KUOCVM_LSU WHERE SO_DU > 0 AND TTHAI_KUOC = 'DTT'";
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                        ds = dsTruyVan.GetData(nativeQuery);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string MA_KUOCVM = ds.Tables[0].Rows[i][0].ToString();
                                string SO_DU = ds.Tables[0].Rows[i][1].ToString();
                                string SO_TIEN_GIAI_NGAN = ds.Tables[0].Rows[i][2].ToString();
                                string GOC_DA_THU = ds.Tables[0].Rows[i][3].ToString();
                                string LAI_PHAI_THU = ds.Tables[0].Rows[i][4].ToString();
                                string LAI_DA_THU = ds.Tables[0].Rows[i][5].ToString();
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "MA_KUOCVM, SO_DU, SO_TIEN_GIAI_NGAN, GOC_DA_THU, LAI_PHAI_THU, LAI_DA_THU");
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, MA_KUOCVM + ", " + SO_DU + ", " + SO_TIEN_GIAI_NGAN + ", " + GOC_DA_THU + ", " + LAI_PHAI_THU + ", " + LAI_DA_THU);
                            }
                            nativeQuery = "UPDATE TD_KUOCVM_LSU SET SO_DU = 0 WHERE SO_DU > 0 AND TTHAI_KUOC = 'DTT'";
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                            ds = dsTruyVan.GetData(nativeQuery);
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, ds.ToString());
                        }
                    }
                }
                
                return SoLuongKheUoc;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public int CapNhatSoTietKiem(ApplicationConstant.DonViSuDung CompanyCode)
        {
            int SoLuongSoTietKiem = 0;
            try
            {
                if (CompanyCode.Equals(ApplicationConstant.DonViSuDung.BINHKHANH))
                {
                    string nativeQuery = "";
                    DS_TruyVan dsTruyVan = new DS_TruyVan();
                    DataSet ds = new DataSet();

                    // Lay so luong trong TD_KUOCVM
                    nativeQuery = "SELECT COUNT(*) FROM BL_TIEN_GUI where DU_CHI_LAI is NULL";
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                    ds = dsTruyVan.GetData(nativeQuery);
                    string SoLuongSo = "";
                    if (ds != null && ds.Tables[0].Rows.Count > 0) SoLuongSo = ds.Tables[0].Rows[0][0].ToString();
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "SoLuongSo: " + SoLuongSo);

                    // Lay so luong trong TD_KUOCVM_LSU
                    nativeQuery = "SELECT COUNT(*) FROM BL_TIEN_GUI_LSU where DU_CHI_LAI is NULL";
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                    ds = dsTruyVan.GetData(nativeQuery);
                    string SoLuongSo_LSU = "";
                    if (ds != null && ds.Tables[0].Rows.Count > 0) SoLuongSo_LSU = ds.Tables[0].Rows[0][0].ToString();
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "SoLuongSo_LSU: " + SoLuongSo_LSU);

                    if (SoLuongSo.Equals(SoLuongSo_LSU) && !SoLuongSo.Equals("0"))
                    {
                        SoLuongSoTietKiem = Int32.Parse(SoLuongSo);

                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "BL_TIEN_GUI PROCESSING");
                        nativeQuery = "SELECT MA_KHANG, SO_SO_TG FROM BL_TIEN_GUI where DU_CHI_LAI is NULL";
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                        ds = dsTruyVan.GetData(nativeQuery);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string MA_KHANG = ds.Tables[0].Rows[i][0].ToString();
                                string SO_SO_TG = ds.Tables[0].Rows[i][1].ToString();
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "MA_KHANG, SO_SO_TG");
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, MA_KHANG + ", " + SO_SO_TG);
                            }
                            nativeQuery = "UPDATE BL_TIEN_GUI SET DU_CHI_LAI = 0 WHERE DU_CHI_LAI is NULL";
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                            ds = dsTruyVan.GetData(nativeQuery);
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, ds.ToString());
                        }


                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "BL_TIEN_GUI_LSU PROCESSING");
                        nativeQuery = "SELECT MA_KHANG, SO_SO_TG FROM BL_TIEN_GUI_LSU where DU_CHI_LAI is NULL";
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                        ds = dsTruyVan.GetData(nativeQuery);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string MA_KHANG = ds.Tables[0].Rows[i][0].ToString();
                                string SO_SO_TG = ds.Tables[0].Rows[i][1].ToString();
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, "MA_KHANG, SO_SO_TG");
                                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, MA_KHANG + ", " + SO_SO_TG);
                            }
                            nativeQuery = "UPDATE BL_TIEN_GUI_LSU SET DU_CHI_LAI = 0 WHERE DU_CHI_LAI is NULL";
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, nativeQuery);
                            ds = dsTruyVan.GetData(nativeQuery);
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.BUS, ds.ToString());
                        }
                    }
                }

                return SoLuongSoTietKiem;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
