using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.IO;

namespace Utilities.Common
{
    /// <summary>
    /// Thư viện chứa các hàm thao tác với Dataset và Datatable
    /// </summary>
    public static class LDatatable
    {
        /// <summary>
        /// Tạo bảng chứa tham số
        /// <para>True: Tạo thành công</para>
        /// <para>False: Tạo không thành công</para>
        /// </summary>
        /// <param name="dt">Bảng chứa tham số</param>
        /// <returns></returns>
        public static bool MakeParameterTable(ref DataTable dt)
        {
            bool bresult = false;
            try
            {
                if (dt == null)
                {
                    dt = new DataTable("UDTT_INPUT_PARAMETERS");
                    dt.Columns.Add("PARAM_NAME", typeof(string));
                    dt.Columns.Add("PARAM_TYPE", typeof(string));
                    dt.Columns.Add("PARAM_VALUE", typeof(string));
                }

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        /// <summary>
        /// Tạo bảng chứa tham số cau truc dien giai
        /// <para>True: Tạo thành công</para>
        /// <para>False: Tạo không thành công</para>
        /// </summary>
        /// <param name="dt">Bảng chứa tham số</param>
        /// <returns></returns>
        public static bool MakeParameterTableDes(ref DataTable dt)
        {
            bool bresult = false;
            try
            {
                if (dt == null)
                {
                    dt = new DataTable("UDTT_INPUT_PARAMETERS_DES");
                    dt.Columns.Add("PARAM_NAME", typeof(string));
                    dt.Columns.Add("PARAM_TYPE", typeof(string));
                    dt.Columns.Add("PARAM_CATE", typeof(string));
                    dt.Columns.Add("PARAM_VALUE", typeof(string));
                }

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        /// <summary>
        /// Thêm tham số vào bảng tham số
        /// <para>True: thêm thành công</para>
        /// <para>False: thêm không thành công</para>
        /// </summary>
        /// <param name="dt">Bảng chứa tham số</param>
        /// <param name="paraName">Giá trị của tên tham số</param>
        /// <param name="paraType">Giá trị của kiểu tham số</param>
        /// <param name="paraValue">Giá trị của giá trị tham số</param>
        /// <returns></returns>
        public static bool AddParameter(ref DataTable dt, string paraName, string paraType, string paraValue)
        {
            bool bresult = false;
            try
            {
                DataRow dr;

                if (dt == null)
                {
                    dt = new DataTable("UDTT_INPUT_PARAMETERS");
                    dt.Columns.Add("PARAM_NAME", typeof(string));
                    dt.Columns.Add("PARAM_TYPE", typeof(string));
                    dt.Columns.Add("PARAM_VALUE", typeof(string));
                }

                dr = dt.NewRow();
                dr["PARAM_NAME"] = paraName;
                dr["PARAM_TYPE"] = paraType;
                dr["PARAM_VALUE"] = paraValue;
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        /// <summary>
        /// Thêm tham số vào bảng tham số
        /// <para>True: thêm thành công</para>
        /// <para>False: thêm không thành công</para>
        /// </summary>
        /// <param name="dt">Bảng chứa tham số</param>
        /// <param name="paraName">Giá trị của tên tham số</param>
        /// <param name="paraType">Giá trị của kiểu tham số</param>
        /// <param name="paraValue">Giá trị của giá trị tham số</param>
        /// <returns></returns>
        public static bool AddParameterDes(ref DataTable dt, string paraName, string paraType, string paraCate, string paraValue)
        {
            bool bresult = false;
            try
            {
                DataRow dr;

                if (dt == null)
                {
                    dt = new DataTable("UDTT_INPUT_PARAMETERS_DES");
                    dt.Columns.Add("PARAM_NAME", typeof(string));
                    dt.Columns.Add("PARAM_TYPE", typeof(string));
                    dt.Columns.Add("PARAM_CATE", typeof(string));
                    dt.Columns.Add("PARAM_VALUE", typeof(string));
                }

                dr = dt.NewRow();
                dr["PARAM_NAME"] = paraName;
                dr["PARAM_TYPE"] = paraType;
                dr["PARAM_CATE"] = paraCate;
                dr["PARAM_VALUE"] = paraValue;
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        /// <summary>
        /// Tạo bảng chứa tham số
        /// <para>True: Tạo thành công</para>
        /// <para>False: Tạo không thành công</para>
        /// </summary>
        /// <param name="dt">Bảng chứa tham số</param>
        /// <returns></returns>
        public static bool MakeParameterListTable(ref DataTable dt)
        {
            bool bresult = false;
            try
            {
                if (dt == null)
                {
                    dt = new DataTable("UDTT_INPUT_PARAMETERS_LIST");
                    dt.Columns.Add("P1", typeof(string));
                    dt.Columns.Add("P2", typeof(string));
                    dt.Columns.Add("P3", typeof(string));
                    dt.Columns.Add("P4", typeof(string));
                    dt.Columns.Add("P5", typeof(string));
                    dt.Columns.Add("P6", typeof(string));
                    dt.Columns.Add("P7", typeof(string));
                    dt.Columns.Add("P8", typeof(string));
                    dt.Columns.Add("P9", typeof(string));
                    dt.Columns.Add("P10", typeof(string));
                    dt.Columns.Add("P11", typeof(string));
                    dt.Columns.Add("P12", typeof(string));
                    dt.Columns.Add("P13", typeof(string));
                    dt.Columns.Add("P14", typeof(string));
                    dt.Columns.Add("P15", typeof(string));
                    dt.Columns.Add("P16", typeof(string));
                    dt.Columns.Add("P17", typeof(string));
                    dt.Columns.Add("P18", typeof(string));
                    dt.Columns.Add("P19", typeof(string));
                    dt.Columns.Add("P20", typeof(string));
                    dt.Columns.Add("P21", typeof(string));
                    dt.Columns.Add("P22", typeof(string));
                    dt.Columns.Add("P23", typeof(string));
                    dt.Columns.Add("P24", typeof(string));
                    dt.Columns.Add("P25", typeof(string));
                    dt.Columns.Add("P26", typeof(string));
                    dt.Columns.Add("P27", typeof(string));
                    dt.Columns.Add("P28", typeof(string));
                    dt.Columns.Add("P29", typeof(string));
                    dt.Columns.Add("P30", typeof(string));
                    dt.Columns.Add("P31", typeof(string));
                    dt.Columns.Add("P32", typeof(string));
                    dt.Columns.Add("P33", typeof(string));
                    dt.Columns.Add("P34", typeof(string));
                    dt.Columns.Add("P35", typeof(string));
                    dt.Columns.Add("P36", typeof(string));
                    dt.Columns.Add("P37", typeof(string));
                    dt.Columns.Add("P38", typeof(string));
                    dt.Columns.Add("P39", typeof(string));
                    dt.Columns.Add("P40", typeof(string));
                    dt.Columns.Add("P41", typeof(string));
                    dt.Columns.Add("P42", typeof(string));
                    dt.Columns.Add("P43", typeof(string));
                    dt.Columns.Add("P44", typeof(string));
                    dt.Columns.Add("P45", typeof(string));
                    dt.Columns.Add("P46", typeof(string));
                    dt.Columns.Add("P47", typeof(string));
                    dt.Columns.Add("P48", typeof(string));
                    dt.Columns.Add("P49", typeof(string));
                    dt.Columns.Add("P50", typeof(string));

                }

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        public static bool AddParameterList(ref DataTable dt
                                        , string p1 = null
                                        , string p2 = null
                                        , string p3 = null
                                        , string p4 = null
                                        , string p5 = null
                                        , string p6 = null
                                        , string p7 = null
                                        , string p8 = null
                                        , string p9 = null
                                        , string p10 = null
                                        , string p11 = null
                                        , string p12 = null
                                        , string p13 = null
                                        , string p14 = null
                                        , string p15 = null
                                        , string p16 = null
                                        , string p17 = null
                                        , string p18 = null
                                        , string p19 = null
                                        , string p20 = null
                                        , string p21 = null
                                        , string p22 = null
                                        , string p23 = null
                                        , string p24 = null
                                        , string p25 = null
                                        , string p26 = null
                                        , string p27 = null
                                        , string p28 = null
                                        , string p29 = null
                                        , string p30 = null
                                        , string p31 = null
                                        , string p32 = null
                                        , string p33 = null
                                        , string p34 = null
                                        , string p35 = null
                                        , string p36 = null
                                        , string p37 = null
                                        , string p38 = null
                                        , string p39 = null
                                        , string p40 = null
                                        , string p41 = null
                                        , string p42 = null
                                        , string p43 = null
                                        , string p44 = null
                                        , string p45 = null
                                        , string p46 = null
                                        , string p47 = null
                                        , string p48 = null
                                        , string p49 = null
                                        , string p50 = null)
        {
            bool bresult = false;
            try
            {
                DataRow dr;

                if (dt == null)
                {                    
                    MakeParameterListTable(ref dt);
                }

                dr = dt.NewRow();
                if (p1 != null) dr["P1"] = p1;
                if (p2 != null) dr["P2"] = p2;
                if (p3 != null) dr["P3"] = p3;
                if (p4 != null) dr["P4"] = p4;
                if (p5 != null) dr["P5"] = p5;
                if (p6 != null) dr["P6"] = p6;
                if (p7 != null) dr["P7"] = p7;
                if (p8 != null) dr["P8"] = p8;
                if (p9 != null) dr["P9"] = p9;
                if (p10 != null) dr["P10"] = p10;
                if (p11 != null) dr["P11"] = p11;
                if (p12 != null) dr["P12"] = p12;
                if (p13 != null) dr["P13"] = p13;
                if (p14 != null) dr["P14"] = p14;
                if (p15 != null) dr["P15"] = p15;
                if (p16 != null) dr["P16"] = p16;
                if (p17 != null) dr["P17"] = p17;
                if (p18 != null) dr["P18"] = p18;
                if (p19 != null) dr["P19"] = p19;
                if (p20 != null) dr["P20"] = p20;
                if (p21 != null) dr["P21"] = p21;
                if (p22 != null) dr["P22"] = p22;
                if (p23 != null) dr["P23"] = p23;
                if (p24 != null) dr["P24"] = p24;
                if (p25 != null) dr["P25"] = p25;
                if (p26 != null) dr["P26"] = p26;
                if (p27 != null) dr["P27"] = p27;
                if (p28 != null) dr["P28"] = p28;
                if (p29 != null) dr["P29"] = p29;
                if (p30 != null) dr["P30"] = p30;
                if (p31 != null) dr["P31"] = p31;
                if (p32 != null) dr["P32"] = p32;
                if (p33 != null) dr["P33"] = p33;
                if (p34 != null) dr["P34"] = p34;
                if (p35 != null) dr["P35"] = p35;
                if (p36 != null) dr["P36"] = p36;
                if (p37 != null) dr["P37"] = p37;
                if (p38 != null) dr["P38"] = p38;
                if (p39 != null) dr["P39"] = p39;
                if (p40 != null) dr["P40"] = p40;
                if (p41 != null) dr["P41"] = p41;
                if (p42 != null) dr["P42"] = p42;
                if (p43 != null) dr["P43"] = p43;
                if (p44 != null) dr["P44"] = p44;
                if (p45 != null) dr["P45"] = p45;
                if (p46 != null) dr["P46"] = p46;
                if (p47 != null) dr["P47"] = p47;
                if (p48 != null) dr["P48"] = p48;
                if (p49 != null) dr["P49"] = p49;
                if (p50 != null) dr["P50"] = p50;

                dt.Rows.Add(dr);
                dt.AcceptChanges();

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        /// <summary>
        /// Tạo bảng chứa lãi suất
        /// <para>True: Tạo thành công</para>
        /// <para>False: Tạo không thành công</para>
        /// </summary>
        /// <param name="dt">Bảng chứa tham số</param>
        /// <returns></returns>
        public static bool TaoBangLaiSuat(ref DataTable dt)
        {
            bool bresult = false;
            try
            {
                if (dt == null)
                {
                    dt = new DataTable("DIEN_GIAI_LAI_SUAT");
                    dt.Columns.Add("MA_DOI_TUONG", typeof(string));
                    dt.Columns.Add("TU_NGAY", typeof(string));
                    dt.Columns.Add("DEN_NGAY", typeof(string));
                    dt.Columns.Add("SO_TIEN_TINH_LAI", typeof(decimal));
                    dt.Columns.Add("LAI_SUAT", typeof(decimal));
                    dt.Columns.Add("SO_NGAY", typeof(int));
                    dt.Columns.Add("KY_HAN", typeof(int));
                    dt.Columns.Add("KY_HAN_DVT", typeof(string));
                    dt.Columns.Add("SO_TIEN_LAI", typeof(decimal));
                }

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        /// <summary>
        /// Thêm diễn giải lãi suất
        /// <para>True: thêm thành công</para>
        /// <para>False: thêm không thành công</para>
        /// </summary>
        /// <param name="dt">Bảng chứa tham số</param>
        /// <param name="maDoiTuong"></param>
        /// <param name="tuNgay"></param>
        /// <param name="denNgay"></param>
        /// <param name="soTienTinhLai"></param>
        /// <param name="laiSuat"></param>
        /// <param name="soNgay"></param>
        /// <param name="soTienLai"></param>
        /// <returns></returns>
        public static bool ThemDienGiaiLaiSuat(ref DataTable dt, string maDoiTuong, string tuNgay, string denNgay,
                                        decimal soTienTinhLai, decimal laiSuat, int soNgay,int kyHan,string kyHanDVT, decimal soTienLai)
        {
            bool bresult = false;
            try
            {
                DataRow dr;

                if (dt == null)
                {
                    bresult = TaoBangLaiSuat(ref dt);
                    if (!bresult) return bresult;
                }

                dr = dt.NewRow();
                dr["MA_DOI_TUONG"] = maDoiTuong;
                dr["TU_NGAY"] = tuNgay;
                dr["DEN_NGAY"] = denNgay;
                dr["SO_TIEN_TINH_LAI"] = soTienTinhLai;
                dr["LAI_SUAT"] = laiSuat;
                dr["SO_NGAY"] = soNgay;
                dr["KY_HAN"] = kyHan;
                dr["KY_HAN_DVT"] = kyHanDVT;
                dr["SO_TIEN_LAI"] = soTienTinhLai;
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                bresult = true;
            }
            catch (Exception ex)
            {
                bresult = false;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

            return bresult;
        }

        public static DataTable XElementToDataTable(XElement x)
        {
            DataTable dt = new DataTable();

            XElement setup = (from p in x.Descendants() select p).First();
            foreach (XElement xe in setup.Descendants()) // build your DataTable
                dt.Columns.Add(new DataColumn(xe.Name.ToString(), typeof(string))); // add columns to your dt

            var all = from p in x.Descendants(setup.Name.ToString()) select p;
            foreach (XElement xe in all)
            {
                DataRow dr = dt.NewRow();
                foreach (XElement xe2 in xe.Descendants())
                    dr[xe2.Name.ToString()] = xe2.Value; //add in the values
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

    }
}
