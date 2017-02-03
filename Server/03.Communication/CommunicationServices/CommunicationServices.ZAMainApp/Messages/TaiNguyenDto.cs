using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CommunicationServices.ZAMainApp.Messages
{
    /// <summary>
    /// TruongNX on 20120927
    /// </summary>
    [DataContract]
    public class TaiNguyenDto
    {
        [DataMember]
        public int IdTaiNguyen;

        [DataMember]
        public string LoaiTaiNguyen;

        [DataMember]
        public string MaTaiNguyen;

        [DataMember]
        public int IdTaiNguyenCha;
    }
}
