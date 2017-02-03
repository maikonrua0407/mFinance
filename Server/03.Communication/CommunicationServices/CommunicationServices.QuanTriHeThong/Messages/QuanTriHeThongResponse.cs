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
    public class LayDSNgayLamViecResponse : ResponseBase
    {
        /// <summary>
        /// Năm cần lấy danh sách
        /// </summary>
        [DataMember]
        public List<bool> ListNgayLamViec;
    }

    /// <summary>
    /// Cập nhật toàn bộ bảng HT_LICH
    /// </summary>
    public class LuuLichResponse : ResponseBase
    {
    }
}
