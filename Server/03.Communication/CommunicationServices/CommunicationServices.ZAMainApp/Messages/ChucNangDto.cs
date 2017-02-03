using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CommunicationServices.ZAMainApp.Messages
{
    /// <summary>
    /// TruongNX on 20120927
    /// </summary>
    [DataContract]
    public class ChucNangDto
    {
        [DataMember]
        public int IDChucNang;

        [DataMember]
        public int IDChucNangCha;

        [DataMember]
        public int STT;

        [DataMember]
        public string TieuDe;

        [DataMember]
        public string ThuocTinh;

        [DataMember]
        public string BieuTuong;

        [DataMember]
        public string ChucNang;

        [DataMember]
        public string PhuongThuc;

        [DataMember]
        public string FormCase;

        [DataMember]
        public int Quyen;

        [DataMember]
        public string MenuHelp;

        [DataMember]
        public string MenuType;

        [DataMember]
        public string UrlGroup;

        [DataMember]
        public string Url;

        [DataMember]
        public string UrlType;

        [DataMember]
        public string UrlICon;

        [DataMember]
        public string UrlHelp;

        [DataMember]
        public string UrlCat;

        [DataMember]
        public List<TinhNangDto> lstTinhNang;
    }
}
