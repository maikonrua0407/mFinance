using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;
using DataModel.EntityFramework;
using Utilities.Common;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    /// <summary>
    /// Lấy danh sách ngày làm việc trong tháng
    /// </summary>
    [DataContract]
    public class LayDSNgayLamViecRequest : RequestBase
    {
        /// <summary>
        /// Năm cần lấy danh sách
        /// </summary>
        [DataMember]
        public int Year;

        /// <summary>
        /// Tháng cần lấy danh sách
        /// </summary>
        [DataMember]
        public int Month;
    }

    /// <summary>
    /// Cập nhật toàn bộ bảng HT_LICH
    /// </summary>
    public class LuuLichRequest : RequestBase
    {
        /// <summary>
        /// Danh sách các object dữ liệu lịch 
        /// </summary>
        [DataMember]
        public List<HT_LICH> ListHT_LICH;
    }
}
