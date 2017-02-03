using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunicationMessages.Base.MessageBases;
using System.Runtime.Serialization;
using BusinessServices.QuanTriHeThong.DTO;
using Utilities.Common;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    public class PhamViRequest : RequestBase
    {
        [DataMember]
        public PHAM_VI objPhamVi;
    }
}
