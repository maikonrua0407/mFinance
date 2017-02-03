using System;
using System.IO;
using System.Web.Mvc;
using System.Windows.Forms;
using Utilities.Common;

namespace PresentationAspNet.MVC.Controllers
{
    [Common.SessionAuthorizeAttribute]
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            return View();
        }

        public ActionResult AccessDenided()
        {
            return View();
        }

        //public ActionResult CauHinhMacDinh()
        //{
        //    var model = UserInformation.Session_User.CauHinhUser;
        //    return PartialView("CauHinhMacDinh", model);
        //}

        public ActionResult LuuCauHinhMacDinh(string idKho, string idDiemBan, string khoInDonHang, string pageSize, string ptThanhToan, string ptGiaoHang, string htGiaoHang, string nvBanHang)
        {
            return Json(Common.ResultJson.Success.LayMa(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BangPhimTat()
        {
            return PartialView("BangPhimTat");
        }

        public ActionResult KeepAlive()
        {
            var result = "0";
            try
            {
                result = "";
            }
            catch (Exception)
            {
                result = "0";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChonDiemGd(int idKho, string idDiemGd)
        {
            var ses = UserInformation.Session_User;
            ses.IdDonViGiaoDich = idKho;
            UserInformation.Session_User = ses;
            return Json(Common.ResultJson.Success.LayMa(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDate()
        {
            var h = DateTime.Now.Hour;
            var m = DateTime.Now.Minute < 10 ? "0" + DateTime.Now.Minute : DateTime.Now.Minute + "";
            var s = DateTime.Now.Second < 10 ? "0" + DateTime.Now.Second : DateTime.Now.Second + "";
            return Json(h + ":" + m + ":" + s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TaiLieuHuongDan(string url)
        {
            var path = Server.MapPath("~/Manual/Manual.chm");

            Help.ShowHelp(null, path, HelpNavigator.Topic, url);
            //Process.Start(path);
            return Json(path, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchMenu(string keyword)
        {
            //var result = RenderPartialHelper.RenderPartialToString(this.ControllerContext, Url.Content("~/Views/Shared/NavigatorMenu.cshtml"), keyword);
            //return Json(result, JsonRequestBehavior.AllowGet);
            return PartialView("NavigatorMenu", keyword);
        }

        public JsonResult ChangeLanguage(string lang)
        {
            string result = "";
            if (UserInformation.Session_User.IsNullOrEmpty())
                UserInformation.Session_User = new Presentation.Process.Common.UserInformation();
            if (UserInformation.Session_User.NgonNgu.IsNullOrEmpty())
                UserInformation.Session_User.NgonNgu = lang;
            else if (UserInformation.Session_User.NgonNgu.ToLower() != lang.ToLower())
            {
                UserInformation.Session_User.NgonNgu = lang;
                result = Common.ResultJson.Success.LayMa();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
