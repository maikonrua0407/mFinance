using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    [DataContract]
    public class PhamViDto
    {
        [DataMember]
        public int IdLoaiPvi;
        [DataMember]
        public string MaLoaiPvi;
        [DataMember]
        public int IdPvi;
        [DataMember]
        public string MaPvi;
        [DataMember]
        public string TenPvi;
    }
}
