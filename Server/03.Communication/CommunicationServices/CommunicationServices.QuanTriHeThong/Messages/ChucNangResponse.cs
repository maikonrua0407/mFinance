using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;
using DataModel.EntityFramework;
using Utilities.Common;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    [DataContract]
    public class ChucNangResponse : ResponseBase
    {
        [DataMember]
        public HT_CNANG htCNang;
    }
}
