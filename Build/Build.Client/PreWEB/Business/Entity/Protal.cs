using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.WebClient.Business
{
    #region [Class PortalInfo]

    public class PortalInfo
    {
        #region [Member Variables]
        private string _Title;
        private string _ICon;
        private string _Keywords;
        private string _CopyRight;
        private string _Address;
        private string _Descriptions;
        private string _TemplateBase;
        #endregion

        #region [Member Properties]

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public string ICon
        {
            get { return _ICon; }
            set { _ICon = value; }
        }

        public string Keywords
        {
            get { return _Keywords; }
            set { _Keywords = value; }
        }

        public string CopyRight
        {
            get { return _CopyRight; }
            set { _CopyRight = value; }
        }

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public string Descriptions
        {
            get { return _Descriptions; }
            set { _Descriptions = value; }
        }

        public string TemplateBase
        {
            get { return _TemplateBase; }
            set { _TemplateBase = value; }
        }

        #endregion

        #region [Constructor]

        public PortalInfo()
        {
        }

        #endregion
    }

    #endregion
}