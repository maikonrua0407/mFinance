﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;

namespace CommunicationServices.ZAMainApp.Messages
{
    [DataContract]
    public class SessionRequest : RequestBase
    {
        [DataMember]
        public string License = "";

        [DataMember]
        public string Version = "";
    }
}
