using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    [DataContract]
    public class DSChucNangDto
    {
        [DataMember]
        public List<string> lstChucNang;
    }
}
