using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities.Common;
using DataModel.EntityFramework;
using DataServices.ZAMainApp;
using DataServices.QuanTriHeThong;
using BusinessServices.Utilities;
using BusinessServices.Utilities.DTO;

namespace BusinessServices.ZAMainApp
{
    public class BS_ZAMainApp
    {
        private static BS_CoDataLogging dataLogging = new BS_CoDataLogging();

        public HT_NSD doLogin(string userName, string passWord)
        {
            DS_ZAMainApp loginService = new DS_ZAMainApp();

            //dataLogging.WriteLog(
            //    DateTime.Now.ToString("M/d/yyyy hh:mm:ss tt"),
            //    new THONG_TIN_CHUNG()
            //    );

            return loginService.doLogin(userName.ToUpper(), passWord);
        }

        public HT_NSD GetNguoiDung(string userName)
        {
            DS_ZAMainApp loginService = new DS_ZAMainApp();
            return loginService.GetNguoiDung(userName.ToUpper());
        }

        public int CapNhatKheUocDaTatToan(ApplicationConstant.DonViSuDung CompanyCode)
        {
            return new DS_ZAMainApp().CapNhatKheUocDaTatToan(CompanyCode);
        }

        public int CapNhatSoTietKiem(ApplicationConstant.DonViSuDung CompanyCode)
        {
            return new DS_ZAMainApp().CapNhatSoTietKiem(CompanyCode);
        }


        public DataSet layTaiNguyenTheoUser(string maDangNhap, string maDonVi)
        {
            DS_PhanQuyen layTaiNguyenService = new DS_PhanQuyen();
            DataSet ds = layTaiNguyenService.LayTNuyenKThacTheoUser(maDangNhap, maDonVi);
            return ds;
        }        

        /// <summary>
        /// Kiểm tra thông tin truy cập của NSD dựa vào địa chỉ MAC và IP. 
        /// Trả về true nếu hợp lệ. 
        /// Trả về false nếu không hợp lệ.        
        /// </summary>
        /// <param name="objNSD"></param>
        /// <param name="diaChiMAC"></param>
        /// <param name="diaChiIP"></param>
        /// <returns></returns>
        public bool KiemTraTruyCap(HT_NSD objNSD, string diaChiMAC, string diaChiIP)
        {
            if (objNSD.HAN_CHE_TRUY_CAP.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
            {
                int hopLeMAC = 0;
                int hopLeIP = 0;
                List<HT_TRUY_CAP> lstNSD = null;
                List<HT_TRUY_CAP> lstNhomNSD = null;

                #region Kiểm tra NSD
                lstNSD = new DS_HT_TRUY_CAP().GetByDoiTuong(objNSD.ID, BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri());
                hopLeMAC = KiemTraHopLeMAC(lstNSD, diaChiMAC);
                hopLeIP = KiemTraHopLeIP(lstNSD, diaChiIP);

                //Nếu cả địa chỉ MAC và IP của NSD hợp lệ -> Hợp lệ
                if (hopLeMAC == 1 && hopLeIP == 1)
                    return true;

                //Nếu chỉ địa chỉ MAC hoặc IP của NSD không hợp lệ -> Không hợp lệ
                else if (hopLeMAC == -1 || hopLeIP == -1)
                    return false;

                #endregion 
               
                #region Kiểm tra nhóm người sử dụng nếu NSD chưa xác định được hợp lệ hay không
                List<HT_NHNSD> lstNhom = new DS_HT_NHNSD().GetByIdNSD(objNSD.ID);

                //Nếu người dùng không thuộc nhóm nào -> không hợp lệ
                if (lstNhom == null || lstNhom.Count == 0)
                {
                    return false;
                }

                //Nếu tồn tại 1 nhóm không hạn chế truy cập -> hợp lệ
                else if (lstNhom.Exists(e => e.HAN_CHE_TRUY_CAP.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri())))
                {
                    return true;
                }

                //Ngược lại nếu tất cả các nhóm đều hạn chế truy cập thì kiểm tra các địa chỉ mà các nhóm đã hạn chế
                else
                {
                    List<int> lstIDNhom = lstNhom.Select(e => e.ID).ToList();
                    lstNhomNSD = new DS_HT_TRUY_CAP().GetByListIdDoiTuong(lstIDNhom, BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri());
                    hopLeMAC = KiemTraHopLeMAC(lstNhomNSD, diaChiMAC);
                    hopLeIP = KiemTraHopLeIP(lstNhomNSD, diaChiIP);

                    //Nếu cả địa chỉ MAC và IP của NSD hợp lệ -> Hợp lệ
                    if (hopLeMAC == 1 && hopLeIP == 1)
                        return true;
                    //Ngược lại -> không hợp lệ
                    else
                        return false;
                }
                #endregion
            }
            else
            {
                return true; //Không hạn chế truy cập -> hợp lệ
            }            
        }

        /// <summary>
        /// Kiểm tra hợp lệ địa chỉ IP. 
        /// Trả về -1 nếu IP bị chặn. 
        /// Trả về 0 nếu IP không bị chặn. 
        /// Trả về 1 nếu IP được phép truy cập
        /// </summary>
        /// <param name="lstTruyCap"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int KiemTraHopLeIP(List<HT_TRUY_CAP> lstTruyCap, string ip)
        {
            lstTruyCap = lstTruyCap.Where(e => e.LOAI_DIA_CHI.Equals(BusinessConstant.LOAI_DIA_CHI.IP.layGiaTri())).ToList();

            if (lstTruyCap == null || lstTruyCap.Count == 0)
                return 0;

            //So sánh với các địa chỉ ip bị chặn
            List<HT_TRUY_CAP> lstKoKichHoat = lstTruyCap.Where(e => e.KICH_HOAT.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri())).ToList();
            foreach (var s in lstKoKichHoat)
            {
                if (SoSanhIP(s.DIA_CHI, ip) == true)
                    return -1;
            }

            //So sánh với các địa chỉ ip được phép truy cập
            List<HT_TRUY_CAP> lstKichHoat = lstTruyCap.Where(e => e.KICH_HOAT.Equals(BusinessConstant.CoKhong.CO.layGiaTri())).ToList();
            foreach (var s in lstKichHoat)
            {
                if (SoSanhIP(s.DIA_CHI, ip) == true)
                    return 1;
            }

            return 0;
        }

        /// <summary>
        /// Kiểm tra hợp lệ địa chỉ MAC. 
        /// Trả về -1 nếu MAC bị chặn. 
        /// Trả về 0 nếu MAC không bị chặn. 
        /// Trả về 1 nếu MAC được phép truy cập
        /// </summary>
        /// <param name="lstTruyCap"></param>
        /// <param name="mac"></param>
        /// <returns></returns>
        public int KiemTraHopLeMAC(List<HT_TRUY_CAP> lstTruyCap, string mac)
        {
            lstTruyCap = lstTruyCap.Where(e => e.LOAI_DIA_CHI.Equals(BusinessConstant.LOAI_DIA_CHI.MAC.layGiaTri())).ToList();

            if (lstTruyCap == null || lstTruyCap.Count == 0)
                return 0;

            //So sánh với các địa chỉ mac bị chặn
            List<HT_TRUY_CAP> lstKoKichHoat = lstTruyCap.Where(e => e.KICH_HOAT.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri())).ToList();
            foreach (var s in lstKoKichHoat)
            {
                if (SoSanhMAC(s.DIA_CHI, mac) == true)
                    return -1;
            }

            //So sánh với các địa chỉ mac được phép truy cập
            List<HT_TRUY_CAP> lstKichHoat = lstTruyCap.Where(e => e.KICH_HOAT.Equals(BusinessConstant.CoKhong.CO.layGiaTri())).ToList();
            foreach (var s in lstKichHoat)
            {
                if (SoSanhMAC(s.DIA_CHI, mac) == true)
                    return 1;
            }

            return 0;
        }

        /// <summary>
        /// So sánh 2 địa chỉ IP. 
        /// Trả về true nếu bằng nhau. 
        /// Trả về false nếu khác nhau
        /// </summary>
        /// <param name="ipSource">IP nguồn, ip có thể có dạng *</param>
        /// <param name="ip">IP cần so sánh</param>
        /// <returns></returns>
        public bool SoSanhIP(string ipSource, string ip)
        {
            if (!LSecurity.IsIPv4Address(ipSource) || !LSecurity.IsIPv4Address(ip))
                return false;

            //IPSource = "*" -> Luôn bằng nhau
            if(ipSource.Equals("*"))
                return true;

            string[] s1 = ipSource.Split('.');
            string[] s2 = ip.Split('.');

            for (int i = 0; i < s1.Length; i++)
            {
                if (!s1[i].Equals("*") && !s1[i].Equals(s2[i])) //IPSource = "192.168.*.*" & IP = "192.168.1.23"  -> bằng nhau
                    return false;
            }

            return true;
        }

        /// <summary>
        /// So sánh 2 địa chỉ MAC. 
        /// Trả về true nếu bằng nhau. 
        /// Trả về false nếu khác nhau
        /// </summary>
        /// <param name="macSource"></param>
        /// <param name="mac"></param>
        /// <returns></returns>
        public bool SoSanhMAC(string macSource, string mac)
        {
            if (!LSecurity.IsMacAddress(macSource) || !LSecurity.IsMacAddress(mac))
                return false;

            if (macSource.Equals("*"))
                return true;

            macSource = macSource.Replace(" ", "");
            macSource = macSource.Replace("-", "");

            mac = mac.Replace(" ", "");
            mac = mac.Replace("-", "");

            if (macSource.Equals(mac))
                return true;
            else
                return false;
        }
    }
}
