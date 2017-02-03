using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModel.EntityFramework;
using Utilities.Common;

namespace DataServices.QuanTriHeThong
{
    /// <summary>
    /// Lịch làm việc, lịch nghỉ
    /// </summary>
    public class DS_HTLich
    {
        /// <summary>
        /// Lấy nội dung bảng HT_LICH
        /// </summary>
        /// <param name="ngay">Chuỗi chứa ngày cần giới hạn theo định dạng chuẩn</param>
        /// <returns>Trả lại danh sách các dòng dữ liệu</returns>
        public List<HT_LICH> LayLich(string ngay)
        {
            List<HT_LICH> htLich = new List<HT_LICH>();
            using (var ctx = new Entities())
            { // Lấy tất cả các dòng là lịch nghỉ, lễ hằng năm và ngày nghỉ/làm việc cụ thể nếu có
                htLich = ctx.HT_LICH.Where(lich => lich.DINH_DANG != ApplicationConstant.defaultDateTimeFormat 
                                                || lich.GIA_TRI.Equals(ngay)).ToList();
            }
            return htLich;
        }

        /// <summary>
        /// Lấy nội dung bảng HT_LICH
        /// </summary>
        /// <param name="year">Năm cần giới hạn</param>
        /// <param name="month">Tháng cần giới hạn</param>
        /// <returns>Trả lại danh sách các dòng dữ liệu</returns>
        public List<HT_LICH> LayLich(int year, int month)
        {
            List<HT_LICH> htLich = new List<HT_LICH>();
            using (var ctx = new Entities())
            { // Lấy tất cả các dòng là lịch nghỉ, lễ hằng năm và ngày nghỉ/làm việc cụ thể nếu có
                htLich = ctx.HT_LICH.AsEnumerable().Where(lich => lich.DINH_DANG != ApplicationConstant.defaultDateTimeFormat
                                               || (lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) >= LDateTime.GetFirstDateOfMonth(year, month)
                                                && lich.GIA_TRI.StringToDate(ApplicationConstant.defaultDateTimeFormat) <= LDateTime.GetLastDateOfMonth(year, month))).ToList();
            }
            return htLich;
        }
    }
}
