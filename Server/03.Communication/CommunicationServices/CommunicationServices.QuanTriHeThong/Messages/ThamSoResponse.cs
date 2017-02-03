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
    public class ThamSoResponse : ResponseBase
    {
        [DataMember]
        public bool bKetQua;
        [DataMember]
        public int iKetQua;
        [DataMember]
        public ApplicationConstant.NghiepVuResponseMessage responseMessage;
        [DataMember]
        public List<HT_TSO> ListThamSo;
        [DataMember]
        public List<HT_TSO_LOAI> ListLoaiThamSo;

        [DataMember]
        public HT_TSO objThamSo;
    }
}
