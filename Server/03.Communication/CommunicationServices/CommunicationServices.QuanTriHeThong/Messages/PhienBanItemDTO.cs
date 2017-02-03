using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    public class PhienBanItemDTO
    {
        [DataMember]
        public HT_PBAN_CTIET HtPbanCtiet;

        [DataMember]
        public HT_PBAN_FILE HtPbanFile;

        [DataMember]
        public FileBase HtPbanData;
    }
}
