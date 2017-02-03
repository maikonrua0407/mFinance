using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationAspNet.MVC
{
    public class Pager
    {

    }

    public class UrlPara
    {
        string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}