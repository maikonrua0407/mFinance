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
    public class NguoiDungResponse : ResponseBase
    {
        [DataMember]
        public bool bKetQua;
        [DataMember]
        public int iKetQua;
        [DataMember]
        public HT_NSD NSD;
        [DataMember]
        public HT_NHNSD NHNSD;
        [DataMember]
        public List<HT_NSD> ListNSD;
        [DataMember]
        public List<HT_NHNSD> ListNHNSD;
        [DataMember]
        public List<DM_DON_VI> ListDonVi;        
        [DataMember]
        public List<HT_TRUY_CAP> lstTruyCap;

        // Danh sách phạm vi dữ liệu người dùng
        [DataMember]
        public List<PhamViDto> listPhamVi;
        // Danh sách phòng giao dịch
        [DataMember]
        public List<DM_DON_VI> ListPhongGD;
    }
}
