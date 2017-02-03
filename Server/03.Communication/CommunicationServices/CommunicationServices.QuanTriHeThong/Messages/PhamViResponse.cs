using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessServices.QuanTriHeThong.DTO;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    public class PhamViResponse : ResponseBase
    {
        [DataMember]
        public bool bKetQua;
        [DataMember]
        public int iKetQua;
        [DataMember]
        public PHAM_VI objPhamVi;
    }
}
