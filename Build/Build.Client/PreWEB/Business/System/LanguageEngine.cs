using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Windows;
using System.Resources;
using System.Windows.Markup;
using System.Xml;
using System.Threading;

namespace Presentation.WebClient.Business
{
    public class LanguageType
    {
        public const int TypeMessage = 1;
        public const int TypeUI = 0;
    }
    public class LanguageEngine
    {
        #region [Variables]

        private static string mv_strLanguagePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Languages\\";
        private static string mv_strCurrentLanguage = "VN";

        private Hashtable mv_htUI = null;
        private Hashtable mv_htMS = null;


        private static LanguageEngine mv_objCurrLanguage = null;

        #endregion


        #region [Constructor]

        private void InitLanguage()
        {
            mv_htUI = new Hashtable();
            mv_htMS = new Hashtable();

            if (Directory.Exists(mv_strLanguagePath))
            {
                if (File.Exists(mv_strLanguagePath + "UIResources_" + mv_strCurrentLanguage + ".xaml"))
                {
                    XmlDocument v_objDoc = new XmlDocument();
                    using (FileStream v_fs = new FileStream(mv_strLanguagePath + "UIResources_" + mv_strCurrentLanguage + ".xaml",FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
                    {
                        using (StreamReader v_sr = new StreamReader(v_fs))
                        {
                            v_objDoc.LoadXml(v_sr.ReadToEnd());
                            v_sr.Close();
                        }
                        v_fs.Close();
                    }

                    foreach (XmlNode v_objNode in v_objDoc.DocumentElement.ChildNodes)
                    {
                        if (!mv_htUI.ContainsKey(v_objNode.Attributes["x:Key"].Value))
                        {
                            mv_htUI.Add(v_objNode.Attributes["x:Key"].Value, v_objNode.InnerXml);
                        }
                    }

                }

                if (File.Exists(mv_strLanguagePath + "MessageResources_" + mv_strCurrentLanguage + ".xaml"))
                {
                    XmlDocument v_objDoc = new XmlDocument();
                    using (FileStream v_fs = new FileStream(mv_strLanguagePath + "MessageResources_" + mv_strCurrentLanguage + ".xaml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader v_sr = new StreamReader(v_fs))
                        {
                            v_objDoc.LoadXml(v_sr.ReadToEnd());
                            v_sr.Close();
                        }
                        v_fs.Close();
                    }

                    foreach (XmlNode v_objNode in v_objDoc.DocumentElement.ChildNodes)
                    {
                        if (!mv_htMS.ContainsKey(v_objNode.Attributes["x:Key"].Value))
                        {
                            mv_htMS.Add(v_objNode.Attributes["x:Key"].Value, v_objNode.InnerXml);
                        }
                    }

                }
            }
        }

        public LanguageEngine(string pv_strLanguage)
        {
            mv_strCurrentLanguage = pv_strLanguage;
            InitLanguage();
        }

        public static LanguageEngine Instance()
        {
            if (mv_strCurrentLanguage!= AppConfig.Language)
            {
                mv_objCurrLanguage = new Business.LanguageEngine(AppConfig.Language);
            }

            if (null == mv_objCurrLanguage)
            {
                mv_objCurrLanguage = new Business.LanguageEngine(AppConfig.Language);
            }

            return mv_objCurrLanguage;
        }

        #endregion

        #region [Methods]

        public string GetContent(int pv_iType, string pv_strKey)
        {
            string v_strContent = pv_strKey;
            if (pv_iType == LanguageType.TypeUI)
            {
                if (mv_htUI.ContainsKey(pv_strKey))
                {
                    v_strContent = mv_htUI[pv_strKey].ToString();
                }
            }
            else if (pv_iType == LanguageType.TypeMessage)
            {
                if (mv_htMS.ContainsKey(pv_strKey))
                {
                    v_strContent = mv_htMS[pv_strKey].ToString();
                }
            }

            return v_strContent;
        }

        #endregion
    }
}