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
    public class NguoiDungRequest : RequestBase
    {
        [DataMember]
        public HT_NSD objNSD;
        [DataMember]
        public HT_NHNSD objNHNSD;
        // Danh sách id người dùng
        [DataMember]
        public List<int> lstIdNSD;
        // Danh sách id nhóm người dùng
        [DataMember]
        public List<int> lstIdNHNSD;
        // Danh sách id người dùng hoặc nhóm người dùng
        [DataMember]
        public List<int> listID;
        // Danh sách địa chỉ MAC or IP
        [DataMember]
        public List<HT_TRUY_CAP> lstTruyCap;

        [DataMember]
        public string maNguoiDung;
        [DataMember]
        public string oldPassword;
        [DataMember]
        public string newPassword;

        // Danh sách phạm vi dữ liệu người dùng
        [DataMember]
        public List<PhamViDto> listPhamVi;
        [DataMember]
        public string maLoaiPhamVi;
    }
}
