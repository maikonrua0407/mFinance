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
    public class ThamSoRequest : RequestBase
    {
        [DataMember]
        public HT_TSO objThamSo;
        [DataMember]
        public HT_TSO_LOAI objLoaiThamSo;
        [DataMember]
        public List<int> listID;

        [DataMember]
        public string loaiThamSo;
        [DataMember]
        public string maDonVi; 
    }
}
