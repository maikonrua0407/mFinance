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
    public class PhanQuyenResponse : ResponseBase
    {
        [DataMember]
        public List<HT_CNANG> ListChucNang;
        [DataMember]
        public List<HT_CNANG_PQUYEN> ListChucNangPhanQuyen;
        [DataMember]
        public List<HT_TNANG> ListTinhNang;
        [DataMember]
        public List<HT_CNANG_TNANG> ListChucNangTinhNang;
        [DataMember]
        public List<HT_TNGUYEN> ListTaiNguyen;
        [DataMember]
        public List<HT_TNGUYEN_KTHAC> ListTaiNguyenKhaiThac;
        [DataMember]
        public bool bKetQua;
    }
}
