using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationAspNet.MVC.Models
{
    public class PagingInfo
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRow { get; set; }
    }
}