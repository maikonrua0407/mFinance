using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using Presentation.Process.Common;

namespace PresentationWPF.ZATestApp
{
    public class SetVisible
    {
        public ArrayList SetVisibleControl(string formNameSpace, string strloai)
        {
            ArrayList lst = new ArrayList();
            List<string> lstItem = new List<string>();
            XElement xe = XElement.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\ShowConfig.xml");
            var form = xe.Elements("form").Where(a=>a.Attribute("Name").Value == formNameSpace);
            var loai = form.Elements("case").Where(a => a.Attribute("Name").Value == strloai).AsQueryable();
            foreach (var item in loai.Elements("control"))
            {
                lstItem = new List<string>();
                lstItem.Add(item.Attribute("Name").Value);
                lstItem.Add(item.Element("property").Attribute("Name").Value);
                lstItem.Add(item.Element("property").Value);
                lst.Add(lstItem);
            }
            return lst;
        }
    }
}
