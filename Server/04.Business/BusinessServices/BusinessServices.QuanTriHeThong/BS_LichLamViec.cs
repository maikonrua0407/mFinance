using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using DataServices.QuanTriHeThong;
using BusinessServices.Utilities;
using Utilities.Common;

namespace BusinessServices.QuanTriHeThong
{
    public class BS_LichLamViec
    {
        /// <summary>
        /// Kiểm tra các ngày trong tháng có phải là ngày làm việc
        /// </summary>
        /// <param name="year">Năm cần kiểm tra</param>
        /// <param name="month">Tháng cần kiểm tra</param>
        /// <returns>Trả lại danh sách theo từng ngày trong tháng chứa giá trị true là ngày làm việc, false là ngày nghỉ</returns>
        public List<bool> LayDSNgayLamViec(int year, int month)
        {
            // Lấy dữ liệu lịch theo tháng
            List<HT_LICH> listHTLich = (new DS_HT_LICH()).LayLich(year, month); 
            // Lấy ngày đầu tháng
            DateTime dt = LDateTime.GetFirstDateOfMonth(year, month);
            List<bool> result = new List<bool>();
            // Kiểm tra từng ngày trong tháng có phải là ngày làm việc
            //for (int i = 0; i < dt.CountDayOfMonth(); i++) result.Add(!dt.PlusDays(i).IsNotWorkingDay(listHTLich));
            for (int i = 0; i < dt.CountDayOfMonth(); i++) result.Add(!(new BS_Utilities().IsNotWorkingDay(dt.PlusDays(i),listHTLich)));
            return result;
        }

        /// <summary>
        /// Cập nhật toàn bộ bảng HT_LICH
        /// </summary>
        /// <param name="value">Danh sách các object dữ liệu lịch</param>
        public void LuuLich(List<HT_LICH> value)
        {
            (new DS_HT_LICH()).LuuLich(value);
        }
    }
}
