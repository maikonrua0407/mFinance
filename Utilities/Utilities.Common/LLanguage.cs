using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;

namespace Utilities.Common
{
    public static class LLanguage
    {
        /// <summary>
        /// Danh sách các ngôn ngữ
        /// </summary>
        public enum Languages
        {
            Vietnamese,
            English
        }

        /// <summary>
        /// Thay đổi ngôn ngữ theo đường dẫn tới folder và tên viết tắt
        /// </summary>
        /// <param name="pathFolderLanguages">đường dẫn tới folder ngôn ngữ</param>
        /// <param name="cultureName">Tên viết tắt ngôn ngữ</param>
        public static bool ApplyLanguage(string pathFolderLanguages, string cultureName)
        {
            bool kq = true;
            if (cultureName != null)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            }
            ResourceDictionary dict = new ResourceDictionary();
            ResourceDictionary dictMessage = new ResourceDictionary();
            try
            {
                // Đổi UIResource
                dict.Source = new Uri(pathFolderLanguages + @"\UIResources_" + cultureName + ".xaml", UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Add(dict);

                //Đổi MessageResource
                dictMessage.Source = new Uri(pathFolderLanguages + @"\MessageResources_" + cultureName + ".xaml", UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Add(dictMessage);
            }
            catch (System.Exception ex)
            {
                kq = false;
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                dict = null;
                dictMessage = null;
            }
            return kq;
        }

        /// <summary>
        /// Tìm giá trị của key trong file resource đã add vào app
        /// </summary>
        /// <param name="ma">Mã Resource</param>
        /// <returns>Giá trị theo key tìm được</returns>
        public static string SearchResourceByKey(string key)
        {
            object obj = Application.Current.TryFindResource(key);
            if (obj == null) return key;
            else return obj.ToString();
        }

        public static string SearchResourceByKey(string key, string[] obj)
        {
            key = LLanguage.SearchResourceByKey(key);
            key = String.Format(key, obj);
            return key;
        }

        /// <summary>
        /// Tìm giá trị của key trong file resource đã add vào app
        /// </summary>
        /// <param name="ma">Mã Resource</param>
        /// <returns>Giá trị theo key tìm được</returns>
        public static string SearchResourceByKey(string key, char pattern)
        {
            string[] str = key.Split(pattern);
            string trueKey = key;
            string trueData = "";
            string message = "";

            if (str.Length > 1)
            {
                trueKey = str[0];
                trueData = str[1];

                message = SearchResourceByKey(trueKey);
                return message + " (" + SearchResourceByKey(DatabaseConstant.layNgonNguBangDuLieu(trueData)) + ")";
            }
            else
            {
                message = SearchResourceByKey(trueKey);
                return message;
            }
            
        }

        /// <summary>
        /// Tìm giá trị của key trong file resource bất kỳ
        /// </summary>
        /// <param name="ma">key cần tìm giá trị</param>
        /// <param name="pathFolderLanguages">đường dẫn tới folder ngôn ngữ</param>
        /// <param name="cultureName">Tên viết tắt ngôn ngữ</param>
        /// <returns></returns>
        public static string SearchResourceByKey(string key, string pathFolderLanguages, string cultureName)
        {
            try
            {
                // Tìm UIResource
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri(pathFolderLanguages + @"\UIResources_" + cultureName + ".xaml");
                object objUI = dict[key];
                if (objUI != null) return objUI.ToString();

                //Tìm MessageResource
                ResourceDictionary dictMessage = new ResourceDictionary();
                dictMessage.Source = new Uri(pathFolderLanguages + @"\MessageResources_" + cultureName + ".xaml");
                object objMessage = dictMessage[key];
                if (objMessage != null) return objMessage.ToString();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            return "";
        }
    }
}
