using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunicationMessages.Base.MessageBases;
using System.Runtime.Serialization;

namespace CommunicationServices.ZAMainApp.Messages
{
    [DataContract]
    public class LogoutResponse : ResponseBase
    {
    }
}
