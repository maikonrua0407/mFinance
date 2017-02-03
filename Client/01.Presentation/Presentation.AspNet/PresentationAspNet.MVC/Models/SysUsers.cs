using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presentation.Process.QuanTriHeThongServiceRef;

namespace PresentationAspNet.MVC.Models
{
    public class SysUsers
    {
        public List<HT_NSD> LstNsd { get; set; }
        public List<HT_NHNSD> LstNhNsd { get; set; }
        public List<AutoCompleteEntry> LstSourceLoaiDTuong { get; set; }
    }
}