using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;
using DataModel.EntityFramework;
using System.Collections;

namespace CommunicationServices.QuanTriHeThong.Messages
{
    [DataContract]
    public class PhanQuyenRequest : RequestBase
    {
        [DataMember]
        public string maDoiTuong;
        [DataMember]
        public string loaiDoiTuong;
        [DataMember]
        public string maTaiNguyen;
        [DataMember]
        public string loaiTaiNguyen;
        [DataMember]
        public List<DSChucNangDto> lstPhanQuyen;
        [DataMember]
        public string nguoiCapNhat;
        [DataMember]
        public string maPhanHe;
        [DataMember]
        public List<int> lstIdChucNang;
        [DataMember]
        public List<int> lstIdTinhNang;
        [DataMember]
        public List<int> lstIdMenu;

        // TrườngNX bổ sung
        [DataMember]
        public int idDoiTuong;
        [DataMember]
        public List<HT_CNANG_PQUYEN> dsCNangPQuyenXoa;
        [DataMember]
        public List<HT_CNANG_TNANG> dsCNangTNangThem;
        [DataMember]
        public string maDonVi;
    }
}
