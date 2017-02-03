using System;
using System.Reflection;
using Presentation.Process.ZAMainAppServiceRef;
using Utilities.Common;

namespace PresentationAspNet.MVC.App_Start
{
    public static class AppConfig
    {
        public static void LoadModule()
        {
            try
            {
                //var mfBl = new ModuleFunctionBl();
                //var lstModule = mfBl.GetListUtilObject();
                //if (lstModule != null && lstModule.Any())
                //{
                //    AppSettings.AddModule(lstModule);
                //}
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public static ChucNangDto GetCurModule()
        {
            var url = UrlUltility.GetUrlWithoutQueryString();
            if (string.IsNullOrEmpty(url))
                return null;
            if (string.Equals(url, "/"))
            {
                url = "/Home";
            }
            var module = GetCurModuleByUrl(url);
            if (module.IsNullOrEmpty())
                url = url.TrimEnd('/');
            module = GetCurModuleByUrl(url);
            return module;
        }

        public static ChucNangDto GetCurModuleByUrl(string url)
        {
            try
            {
                var rtn = new ChucNangDto();
                var utilObject = UrlUltility.GetSingleModule(url);
                rtn = Common.Map<ChucNangDto>(utilObject);
                return rtn;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
                return null;
            }
        }

    }
}