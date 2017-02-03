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
    public class PhienBanRequest : RequestBase
    {
        [DataMember]
        public string ClientVersion;
        [DataMember]
        public string LastestClientVersion;

        [DataMember]
        public HT_PBAN HtPban;
        [DataMember]
        public HT_PBAN_CTIET HtPbanCtiet;
        [DataMember]
        public List<HT_PBAN_CTIET> ListHtPbanCtiet;
    }
}
