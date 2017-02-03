using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    [DataContract]
    public class PhienBanDTO
    {
        [DataMember]
        public HT_PBAN HtPban;

        [DataMember]
        public List<PhienBanItemDTO> ListPhienBanItemDTO;
    }
}
