using System;
using System.Linq;
using System.Web.Mvc;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.ZAMainAppServiceRef;
using Utilities.Common;

namespace PresentationAspNet.MVC.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Logoff()
        {
            Session.RemoveAll();
            return Json("LOGOUT", JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoiKhoLamViec(int idDonViGd)
        {
            UserInformation.Session_User.IdDonViGiaoDich = idDonViGd;
            return Json(DataCombobox.LoadComboDiemGdByUser(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckLogin(string user, string password, int width, int height)
        {
            try
            {
                if (fcnValidForm(user, password))
                {
                    ZAMainAppProcess obj = new ZAMainAppProcess();
                    string sMessage = "";
                    string ngongu = "";
                    NGON_NGU_DTO ngonNguDto = new NGON_NGU_DTO();
                    string sMatKhau = LSecurity.MD5Encrypt(password);

                    if (UserInformation.Session_User.IsNullOrEmpty())
                        UserInformation.Session_User = new Presentation.Process.Common.UserInformation();
                    if (UserInformation.Session_User.NgonNgu.IsNullOrEmpty())
                        UserInformation.Session_User.NgonNgu = "vi";
                    else
                        ngongu = UserInformation.Session_User.NgonNgu;
                    UserInformation.Session_User.U_PhienBan = LanguageNode.GetValueUILanguage("U.PhienBan");
                    UserInformation.Session_User.M_PhienBan = LanguageNode.GetValueMessageLanguage("M.PhienBan");
                    System.Web.HttpContext.Current.Session["UserInformation"] = UserInformation.Session_User;
                    bool isResult = obj.doLoginWithSession(user, sMatKhau, ref ngonNguDto, ref sMessage);
                    UserInformation.Session_User = (Presentation.Process.Common.UserInformation)System.Web.HttpContext.Current.Session["UserInformation"];
                    UserInformation.Session_User.NgonNgu = ngongu;
                    if (!ngonNguDto.IsNullOrEmpty())
                    {
                        if (!ngonNguDto.lstResource.IsNullOrEmpty() && ngonNguDto.lstResource.Count() > 0)
                            LanguageNode.LoadUILanguage(ngonNguDto.lstResource.ToList());
                        if (!ngonNguDto.lstMessage.IsNullOrEmpty() && ngonNguDto.lstMessage.Count() > 0)
                            LanguageNode.LoadMessageLanguage(ngonNguDto.lstMessage.ToList());
                    }
                    //if (sMessage != null && (sMessage.Equals(Utilities.Solution.ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhauLanDau.layGiaTri()) || sMessage.Equals(Utilities.Solution.ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhau.layGiaTri())))
                    //{
                    //    ses.USER_ID = user;
                    //    ses.USER_NAME = objUser.USER_NAME;
                    //    ses.FULL_NAME = objUser.FULL_NAME;
                    //    ses.MAT_KHAU = password;
                    //    ses.PHAI_DOI_MK = 1;
                    //    SessionUser.Session_User = ses;
                    //    return Json(Common.ResultJson.Success.LayMa(), JsonRequestBehavior.AllowGet);
                    //}
                    if (isResult)
                    {
                        //ses.PHAI_DOI_MK = 0;
                        //ses.USER_ID = user;
                        //ses.USER_NAME = objUser.TenDangNhap;
                        //ses.FULL_NAME = objUser.HoTen;
                        //ses.DON_VI_TAO = objUser.DON_VI_QLY;
                        //Presentation.Process.DanhMucServiceRef.DM_DON_VI objDV = objdmdv.getDonViByMa(objUser.DON_VI_QLY);
                        //ses.ID_DON_VI_TAO = objDV.ID;
                        //ses.DON_VI_QLY = objDV.MA_DVI_CHA;
                        //ses.LOAI_DON_VI = objDV.LOAI_DVI;
                        //ses.TEN_DON_VI = objDV.TEN_GDICH;
                        //ses.MAT_KHAU = password;
                        //ses.LIST_CHUC_NANG = objUser.ListChucNang;
                        //double dbCount = double.Parse(Application["access_count"].ToString());
                        //dbCount = dbCount + 1;
                        //Application["access_count"] = dbCount;
                        //SessionUser.Session_User = ses;
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(Common.ResultJson.Error.LayMa(), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(Common.ResultJson.Error.LayMa(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(Common.ResultJson.Error.LayMa(), JsonRequestBehavior.AllowGet);
            }
        }

        private bool fcnValidForm(string user, string password)
        {
            bool v_ReturnValue = true;

            if (user.Length == 0)
            {
                v_ReturnValue = false;
            }
            else if (password.Length == 0)
            {
                v_ReturnValue = false;
            }
            else if (password.Length < 6)
            {
                v_ReturnValue = false;
            }
            return v_ReturnValue;
        }

        public ActionResult LoadKho()
        {
            return PartialView("PhongGd");
        }

        public ActionResult ChonPhongGd(string id)
        {
            ViewBag.PhongGd = id;
            return PartialView("PhongGd");
        }

        public JsonResult ChonDiemGd(int idPhongGd)
        {
            // Change something
            try
            {
                UserInformation.Session_User.IdDonViGiaoDich = idPhongGd;
                UserInformation.Session_User.MaDonViGiaoDich = UserInformation.Session_User.ListPhongGD.FirstOrDefault(e => e.ID == idPhongGd).MA_DVI;
                UserInformation.Session_User.TenDonViGiaoDich = UserInformation.Session_User.ListPhongGD.FirstOrDefault(e => e.ID == idPhongGd).TEN_GDICH;
                return Json(Common.ResultJson.Success.LayMa(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(Common.ResultJson.Error.LayMa(), JsonRequestBehavior.AllowGet);
            }
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
