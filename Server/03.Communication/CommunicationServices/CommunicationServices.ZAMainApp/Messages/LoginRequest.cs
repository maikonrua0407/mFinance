using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;

namespace CommunicationServices.ZAMainApp.Messages
{
    [DataContract]
    public class LoginRequest : RequestBase
    {
        [DataMember]
        public string PassWord = "";

        [DataMember]
        public string Language = "";

        [DataMember]
        public string License = "";

        [DataMember]
        public string Version = "";

        [DataMember]
        public string PhienBanResource = "";

        [DataMember]
        public string PhienBanMessage = "";
    }
}
