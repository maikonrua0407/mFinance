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
    public class TinhNangDto
    {
        [DataMember]
        public string MaTinhNang;
    }
}
