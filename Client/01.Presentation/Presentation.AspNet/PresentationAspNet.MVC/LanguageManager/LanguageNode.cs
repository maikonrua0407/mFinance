using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;
using Presentation.Process.ZAMainAppServiceRef;
using Utilities.Common;

namespace PresentationAspNet.MVC
{
    public class LanguageNode
    {

        private static string pathUI = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["pathLangUI"]);
        private static string pathMessage = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["pathLangMessage"]);

        public static string GetValueUILanguage(string ma)
        {
            string lang = UserInformation.Session_User.NgonNgu;
            return GetValueLanguage(pathUI, lang, ma);
        }
        public static string GetValueMessageLanguage(string ma)
        {
            string lang = UserInformation.Session_User.NgonNgu;
            return GetValueLanguage(pathMessage, lang, ma);
        }
        
        private static string GetValueLanguage(string path, string lang, string ma)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            if (lang.IsNullOrEmpty() || lang == "vi") lang = "VIET_NAM";
            else lang = "ENGLISH";
            XmlNode xnList = xml.SelectSingleNode("/NGON_NGUS/NGON_NGU[@MA='" + ma + "']/" + lang);
            return xnList.IsNullOrEmpty() ? null : xnList.InnerText;
        }

        public static void LoadUILanguage(List<NGON_NGU> ngonNgu)
        {
            LoadLanguageToXml(pathUI, ngonNgu);
        }
        public static void LoadMessageLanguage(List<NGON_NGU> ngonNgu)
        {
            LoadLanguageToXml(pathMessage, ngonNgu);
        }

        private static void LoadLanguageToXml(string path, List<NGON_NGU> ngonNgu)
        {
            try
            {
                var xEle = new XElement("NGON_NGUS",
                            from emp in ngonNgu
                            select new XElement("NGON_NGU",
                                         new XAttribute("MA", emp.MA),
                                           new XElement("VIET_NAM", emp.VIET_NAM),
                                           new XElement("ENGLISH", emp.ENGLISH)
                                       ));

                xEle.Save(path);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
