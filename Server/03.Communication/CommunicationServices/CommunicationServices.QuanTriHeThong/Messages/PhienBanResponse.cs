using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;
using DataModel.EntityFramework;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    [DataContract]
    public class PhienBanResponse : ResponseBase
    {
        [DataMember]
        public string ClientVersion;
        [DataMember]
        public string ServerVersion;
        [DataMember]
        public string LastestClientVersion;

        [DataMember]
        public HT_PBAN HtPban;
        [DataMember]
        public List<HT_PBAN_CTIET> ListHtPbanCtiet;
        [DataMember]
        public List<HT_PBAN_FILE> ListHtPbanFile;

        [DataMember]
        public PhienBanDTO PhienBanDTO;
        [DataMember]
        public PhienBanItemDTO PhienBanItemDTO;
    }
}
